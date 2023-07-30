namespace shareme_backend.DTOs.Post;

using System.Text.Json;

public class ListPostsQueryDTO
{
    public int Skip { get; set; } = 0;

    public int Limit { get; set; } = 10;

    public string? Category { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
