
namespace shareme_backend.DTOs.User;

using System.Text.Json;
using shareme_backend.Enums;

public class GetProfileQuery
{
    public TypeProfilePosts Type { get; set; } = TypeProfilePosts.CREATED;

    public int skip { get; set; } = 0;

    public int limit { get; set; } = 10;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
