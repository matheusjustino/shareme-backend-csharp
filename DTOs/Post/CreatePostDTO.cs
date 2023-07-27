namespace shareme_backend.DTOs.Post;

using System.ComponentModel.DataAnnotations;
using System.Text.Json;

public class CreatePostDTO
{
    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public IFormFile Image { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
