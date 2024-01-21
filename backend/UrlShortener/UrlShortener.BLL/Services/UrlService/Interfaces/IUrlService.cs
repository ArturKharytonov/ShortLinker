using UrlShortener.BLL.ViewModels;
using UrlShortener.DAL.Entities;

namespace UrlShortener.BLL.Services.UrlService.Interfaces;

public interface IUrlService
{
    Task<string?> GetLongUrl(string id);
    Task<bool> CreateAsync(string url, int userId, string baseUrl);
    Task<bool> DeleteAsync(string id);
    Task<Url?> GetUrl(string id);
    Task<UrlGridViewModel> GetPageAsync(int currentPage);
}