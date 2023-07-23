namespace shareme_backend.Services;

using shareme_backend.Models;

public interface IJwtService
{
    string GenerateToken(User user);
}
