namespace shareme_backend.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using shareme_backend.Models;
using shareme_backend.Utils;


public class JwtService : IJwtService
{
    public string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(Env.Secret);
        var tokenConfig = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Username", user.Username),
                new Claim("Email", user.Email),
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenConfig);

        return tokenHandler.WriteToken(token);
    }
}
