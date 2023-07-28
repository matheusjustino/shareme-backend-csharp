namespace shareme_backend.DTOs.Post;

using System.Text.Json;
using shareme_backend.DTOs.User;

public class CommentDTO
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public UserDTO User { get; set; }

    public PostDTO Post { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
