namespace UrlShortener.BLL.Services.JwtService.Interfaces;

public interface IJwtService
{
    string GetToken(int id, string username, string role);
}