namespace shareme_backend.Utils;

using AutoMapper;
using shareme_backend.DTOs.Post;
using shareme_backend.DTOs.User;
using shareme_backend.Models;

public class ModelToDTOMapping : Profile
{
    public ModelToDTOMapping()
    {
        CreateMap<User, UserDTO>();
        CreateMap<Post, PostDTO>();
        CreateMap<Post, ListPostsResponseDTO>();
        CreateMap<Comment, CommentDTO>();
    }
}
