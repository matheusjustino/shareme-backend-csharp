namespace shareme_backend.DTOs.User;

using shareme_backend.DTOs.Post;

public class UserProfile
{
    public UserDTO User { get; set; }

    public List<ListPostsResponseDTO> Posts { get; set; }
}
