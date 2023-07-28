namespace shareme_backend.DTOs.Post;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class CreateCommentDTO
{
    [Required]
    public Guid PostId { get; set; }

    [Required]
    public string Content { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
