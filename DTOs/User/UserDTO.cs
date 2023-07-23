namespace shareme_backend.DTOs.User;

using shareme_backend.Models;

public class UserDTO
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string? Avatar { get; set; }

    public string? ProfileImg { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public UserDTO() { }

    public UserDTO(User user)
    {
        this.Id = user.Id;
        this.Username = user.Username;
        this.Email = user.Email;
        this.Avatar = user.Avatar;
        this.ProfileImg = user.ProfileImg;
        this.CreatedAt = user.CreatedAt;
        this.UpdatedAt = user.UpdatedAt;
    }
}