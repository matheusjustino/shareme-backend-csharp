namespace shareme_backend.Services;

using shareme_backend.DTOs.User;

public interface IUserService
{
    Task<List<string>> ListUsernames(string username);
    Task<UserProfile> GetUserProfile(string username, GetProfileQuery data);
}
