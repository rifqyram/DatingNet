using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Enigma.DatingNet.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Enigma.DatingNet.Securities;

public class JwtUtils : IJwtUtils
{
    private readonly IConfiguration _configuration;

    public JwtUtils(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(MemberUserAccess user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration["JwtSettings:Audience"],
            Issuer = _configuration["JwtSettings:Issuer"],
            Expires = DateTime.Now.AddHours(int.Parse(_configuration["JwtSettings:ExpiresInMinutes"])),
            IssuedAt = DateTime.Now,
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.MemberId.ToString()),
                new(ClaimTypes.Name, user.Username),
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}