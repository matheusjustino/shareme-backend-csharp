namespace shareme_backend.Services;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using shareme_backend.Data;
using shareme_backend.DTOs.Post;
using shareme_backend.DTOs.User;
using shareme_backend.Enums;
using shareme_backend.Models;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;

    private readonly AppDbContext _context;

    private readonly IMapper _mapper;

    public UserService(ILogger<UserService> logger, AppDbContext context, IMapper mapper)
    {
        this._logger = logger;
        this._context = context;
        this._mapper = mapper;
    }

    public async Task<UserProfile> GetUserProfile(string username, GetProfileQuery data)
    {
        this._logger.LogInformation($"Get User Profile - username: {username} - data: {data.ToString()}");

        var user = await this._context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            throw new BadHttpRequestException("User not found");
        }

        IQueryable<Post> postsQuery = this._context.Posts;

        switch (data.Type)
        {
            case TypeProfilePosts.LIKED:
            {
                var likedPostIds = await this._context.Likes
                    .Where(l => l.LikedById == user.Id)
                    .Select(l => l.PostId)
                    .ToListAsync();

                postsQuery = postsQuery.Where(p => likedPostIds.Contains(p.Id));
                break;
            }

            case TypeProfilePosts.CREATED:
                postsQuery = postsQuery.Where(p => p.PostedById == user.Id);
                break;

            default:
                postsQuery = this._context.Posts;
                break;
        }

        var posts = await postsQuery
            .OrderByDescending(p => p.CreatedAt)
            .Skip(data.skip * data.limit)
            .Take(data.limit)
            .ToListAsync();

        var userProfile = new UserProfile
        {
            User = this._mapper.Map<UserDTO>(user),
            Posts = this._mapper.Map<List<ListPostsResponseDTO>>(posts),
        };

        return userProfile;
    }

    public async Task<List<string>> ListUsernames(string username)
    {
        this._logger.LogInformation($"List Usernames - username: {username}");

        var usernames = await this._context.Users
            .Where(u => u.Username.Contains(username))
            .Select(u => u.Username)
            .ToListAsync();

        return usernames;
    }
}
