namespace shareme_backend.Services;

using shareme_backend.DTOs.Post;

public interface IPostService
{
    Task<PostDTO> CreatePost(Guid userId, CreatePostDTO data);

    Task<List<ListPostsResponseDTO>> ListPosts(ListPostsQueryDTO query);

    Task<PostDTO> GetPost(Guid postId, Guid? userId);

    Task<CommentDTO> CreateComment(Guid userId, CreateCommentDTO data);

    Task LikeDislikePost(Guid userId, Guid postId);

    string GetFile(string filename);
}
