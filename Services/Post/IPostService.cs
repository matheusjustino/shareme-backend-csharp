namespace shareme_backend.Services;

using shareme_backend.DTOs.Post;

public interface IPostService
{
    Task<PostDTO> CreatePost(Guid userId, CreatePostDTO data);

    Task<List<ListPostsResponseDTO>> ListPosts(int skip = 0, int limit = 10);

    Task<PostDTO> GetPost(Guid postId, Guid? userId);

    Task<CommentDTO> CreateComment(Guid userId, CreateCommentDTO data);

    Task LikeDislikePost(Guid userId, Guid postId);

    string GetFile(string filename);
}
