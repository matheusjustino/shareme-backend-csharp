namespace shareme_backend.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using shareme_backend.Data;
using shareme_backend.DTOs.Post;
using shareme_backend.Models;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;

    private readonly AppDbContext _context;

    private readonly IManageImageService _manageImageService;

    private readonly IMapper _mapper;

    public PostService(ILogger<PostService> logger, AppDbContext context, IManageImageService manageImageService, IMapper mapper)
    {
        this._logger = logger;
        this._context = context;
        this._manageImageService = manageImageService;
        this._mapper = mapper;
    }

    public async Task<PostDTO> CreatePost(Guid userId, CreatePostDTO data)
    {
        this._logger.LogInformation($"Create Post - data: {data.ToString()}");

        var user = await this._context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new BadHttpRequestException("User not found");
        }

        await using var transaction = await this._context.Database.BeginTransactionAsync();
        var filename = await this._manageImageService.UploadFile(data.Image);

        try
        {
            var newPost = new Post
            {
                Title = data.Title, Description = data.Description, ImageSrc = filename, PostedById = userId, User = user,
            };

            await this._context.Posts.AddAsync(newPost);
            await this._context.SaveChangesAsync();
            await transaction.CommitAsync();

            var response = this._mapper.Map<PostDTO>(newPost);

            return response;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            if (!string.IsNullOrEmpty(filename))
            {
                this._manageImageService.DeleteImage(filename);
            }

            throw;
        }
    }

    public async Task<List<ListPostsResponseDTO>> ListPosts(int skip, int limit)
    {
        return this._mapper.Map<List<ListPostsResponseDTO>>(await this._context.Posts
            .OrderBy(p => p.CreatedAt)
            .Skip(skip * limit)
            .Take(limit)
            .ToListAsync());
    }

    public async Task<PostDTO> GetPost(Guid postId)
    {
        this._logger.LogInformation($"Get Post - postId: {postId}");

        var post = await this._context.Posts
            .Where(p => p.Id == postId)
            .Include(p => p.User)
            .FirstOrDefaultAsync();
        if (post is null)
        {
            throw new BadHttpRequestException("Post not found");
        }

        var postComments =
            await this._context.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .Select(c => this._mapper.Map<CommentDTO>(c))
                .ToListAsync();

        var postLikes = await this._context.Likes
            .Select(l => l.PostId == postId)
            .CountAsync();

        var response = this._mapper.Map<PostDTO>(post);
        response.Comments = postComments;
        response.LikesCount = postLikes;

        return response;
    }

    public async Task<CommentDTO> CreateComment(Guid userId, CreateCommentDTO data)
    {
        this._logger.LogInformation($"Create Comment - userId: {userId} - data: {data.ToString()}");

        var newComment = new Comment { PostId = data.PostId, Content = data.Content, CommentedById = userId, };

        await this._context.Comments.AddAsync(newComment);
        await this._context.SaveChangesAsync();

        var comment = await this._context.Comments
            .Include(c => c.User)
            .Select(c => this._mapper.Map<CommentDTO>(c))
            .FirstOrDefaultAsync();
        if (comment is null)
        {
            throw new BadHttpRequestException("Comment not found");
        }

        return comment;
    }

    public async Task LikeDislikePost(Guid userId, Guid postId)
    {
        this._logger.LogInformation($"LikeDislike Post - userId: {userId} - postId: {postId}");

        var existingLike = await this._context.Likes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.LikedById == userId);
        if (existingLike != null)
        {
            this._context.Likes.Remove(existingLike);
        }
        else
        {
            var newLike = new Like { PostId = postId, LikedById = userId, };
            await this._context.AddAsync(newLike);
        }

        await this._context.SaveChangesAsync();
    }

    public string GetFile(string filename)
    {
        return this._manageImageService.GetFile(filename);
    }
}
