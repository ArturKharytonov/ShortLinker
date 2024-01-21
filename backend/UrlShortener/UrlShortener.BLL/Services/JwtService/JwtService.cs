using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UrlShortener.BLL.Services.JwtService.Interfaces;

namespace UrlShortener.BLL.Services.JwtService;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetToken(int id, string username, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, id.ToString()),
            new(ClaimTypes.Name, username),
            new(ClaimTypes.Role, role)
        };
        var now = DateTime.UtcNow;
        var jwtToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            notBefore: now,
            claims: claims,
            expires: now.Add(TimeSpan.FromMinutes(45)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}