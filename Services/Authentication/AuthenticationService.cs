namespace shareme_backend.Services;

using Microsoft.EntityFrameworkCore;
using shareme_backend.Data;
using shareme_backend.DTOs.Auth;
using shareme_backend.DTOs.User;
using shareme_backend.Models;


public class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;

    private readonly AppDbContext _context;

    private readonly IPasswordHasherService _passwordHasherService;

    private readonly IJwtService _jwtService;

    public AuthenticationService(ILogger<AuthenticationService> logger, AppDbContext context, IPasswordHasherService passwordHasherService, IJwtService jwtService)
    {
        this._logger = logger;
        this._context = context;
        this._passwordHasherService = passwordHasherService;
        this._jwtService = jwtService;
    }

    public async Task<UserDTO> Register(RegisterDTO data)
    {
        this._logger.Log(LogLevel.Information, $"Register - data: ${data}");

        var hashedPassword = this._passwordHasherService.Hash(data.Password);

        var newUser = new User
        {
            Username = data.Username,
            Email = data.Email,
            Password = hashedPassword,
        };

        await this._context.Users.AddAsync(newUser);
        await this._context.SaveChangesAsync();

        return new UserDTO(newUser);
    }

    public async Task<string> DoLogin(DoLoginDTO data)
    {
        this._logger.Log(LogLevel.Information, $"Do Login - data: ${data.ToString()}");

        var user = await this._context.Users.FirstOrDefaultAsync(u => u.Email == data.Email);
        if (user is null)
        {
            throw new BadHttpRequestException("Invalid crendentials");
        }

        var validPassword = this._passwordHasherService.Verify(user.Password, data.Password);
        if (!validPassword)
        {
            throw new BadHttpRequestException("Invalid crendentials");
        }

        return this._jwtService.GenerateToken(user);
    }
}
