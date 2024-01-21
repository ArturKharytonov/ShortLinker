namespace UrlShortener.BLL.Services.UserContext.Interfaces;

public interface IUserContext
{
    string? GetUserId();
    string? GetUserRole();
}