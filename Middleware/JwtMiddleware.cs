namespace shareme_backend.Middleware;

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using shareme_backend.DTOs.Auth;
using shareme_backend.Utils;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
    {
        this._next = next;
        this._logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            this.AttachUserToContext(context, token);
        }

        await this._next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.Secret));

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero, // set clockskew to zero so tokens expire exactly at token expiration time.
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            // attach user to context on successful jwt validation
            context.Items["User"] = new CurrentUser
            {
                UserId = new Guid(jwtToken.Claims.First(x => x.Type == "UserId").Value),
                Username = jwtToken.Claims.First(x => x.Type == "Username").Value,
                Email = jwtToken.Claims.First(x => x.Type == "Email").Value,
            };
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex.Message, ex.StackTrace);
            throw new UnauthorizedAccessException("Invalid JWT Token");
        }
    }
}
