namespace shareme_backend.Services;

using shareme_backend.DTOs.Auth;
using shareme_backend.DTOs.User;

public interface IAuthenticationService
{
    Task<UserDTO> Register(RegisterDTO data);

    Task<string> DoLogin(DoLoginDTO data);
}
