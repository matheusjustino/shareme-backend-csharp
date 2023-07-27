namespace shareme_backend.Services;

using shareme_backend.DTOs.Post;
using shareme_backend.Models;

public interface IPostService
{
    Task<Models.Post> CreatePost(Guid userId, CreatePostDTO data);

    Task<List<Post>> ListPosts(int skip = 0, int limit = 10);

    string GetFile(string filename);
}
