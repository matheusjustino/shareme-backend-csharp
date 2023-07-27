namespace shareme_backend.Services;

using Microsoft.EntityFrameworkCore;
using shareme_backend.Data;
using shareme_backend.DTOs.Post;
using shareme_backend.Models;

public class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;

    private readonly AppDbContext _context;

    private readonly IManageImageService _manageImageService;

    public PostService(ILogger<PostService> logger, AppDbContext context, IManageImageService manageImageService)
    {
        this._logger = logger;
        this._context = context;
        this._manageImageService = manageImageService;
    }

    public async Task<Post> CreatePost(Guid userId, CreatePostDTO data)
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
            var newPost = new Models.Post
            {
                Title = data.Title, Description = data.Description, ImageSrc = filename, PostedById = userId, User = user,
            };

            await this._context.Posts.AddAsync(newPost);
            await this._context.SaveChangesAsync();
            await transaction.CommitAsync();

            return newPost;
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

    public async Task<List<Post>> ListPosts(int skip, int limit)
    {
        return await this._context.Posts
            .OrderBy(p => p.CreatedAt)
            .Skip(skip * limit)
            .Take(limit)
            .Include(p => p.User)
            .ToListAsync();
    }

    public string GetFile(string filename)
    {
        return this._manageImageService.GetFile(filename);
    }
}
