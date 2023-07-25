using Microsoft.EntityFrameworkCore;

namespace shareme_backend.Services.Post;

using shareme_backend.Data;
using shareme_backend.DTOs.Post;

public class PostService
{
    private readonly ILogger<PostService> _logger;

    private readonly AppDbContext _context;

    public PostService(ILogger<PostService> logger, AppDbContext context)
    {
        this._logger = logger;
        this._context = context;
    }

    public async Task<Models.Post> CreatePost(Guid userId, CreatePostDTO data)
    {
        this._logger.LogInformation($"Create Post - data: {data.ToString()}");

        var user = await this._context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new BadHttpRequestException("User not found");
        }

        var newPost = new Models.Post
        {
            Title = data.Title, Description = data.Description, ImageSrc = "imageSrc", PostedById = userId, User = user,
        };

        await this._context.Posts.AddAsync(newPost);
        await this._context.SaveChangesAsync();

        return newPost;
    }
}
