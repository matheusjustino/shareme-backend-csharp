namespace shareme_backend.Services;

using shareme_backend.DTOs.Post;

public interface IPostService
{
    Task<PostDTO> CreatePost(Guid userId, CreatePostDTO data);

    Task<List<PostDTO>> ListPosts(int skip = 0, int limit = 10);

    string GetFile(string filename);
}
