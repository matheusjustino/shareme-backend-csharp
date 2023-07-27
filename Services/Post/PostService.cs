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

    public async Task<List<PostDTO>> ListPosts(int skip, int limit)
    {
        return this._mapper.Map<List<PostDTO>>(await this._context.Posts
            .OrderBy(p => p.CreatedAt)
            .Skip(skip * limit)
            .Take(limit)
            .Include(p => p.User)
            .ToListAsync());
    }

    public string GetFile(string filename)
    {
        return this._manageImageService.GetFile(filename);
    }
}
