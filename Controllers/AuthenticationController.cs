namespace shareme_backend.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shareme_backend.DTOs.Auth;
using shareme_backend.DTOs.User;
using shareme_backend.Services;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        this._authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> Register([FromBody] RegisterDTO body)
    {
        var newUser = await this._authenticationService.Register(body);
        return Ok(newUser);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> DoLogin([FromBody] DoLoginDTO body)
    {
        var token = await this._authenticationService.DoLogin(body);
        return Ok(token);
    }

    [Authorize]
    [HttpGet("authenticated")]
    public ActionResult<string> Test()
    {
        var user = (CurrentUser)HttpContext.Items["User"];
        return Ok(user);
    }
}
