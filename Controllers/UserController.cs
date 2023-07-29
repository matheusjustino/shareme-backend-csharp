namespace shareme_backend.Controllers;

using Microsoft.AspNetCore.Mvc;
using shareme_backend.DTOs.User;
using shareme_backend.Services;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        this._userService = userService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<string>>> ListUsernames([FromQuery] ListUsernamesQuery query)
    {
        var usernames = await this._userService.ListUsernames(query.username);
        return Ok(usernames);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserProfile>> GetUserProfile([FromRoute] string username, [FromQuery] GetProfileQuery query)
    {
        var userProfile = await this._userService.GetUserProfile(username, query);
        return Ok(userProfile);
    }
}
