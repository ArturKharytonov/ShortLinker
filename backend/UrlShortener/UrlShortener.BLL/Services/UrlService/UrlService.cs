using Microsoft.EntityFrameworkCore;
using UrlShortener.BLL.Services.UrlService.Interfaces;
using UrlShortener.BLL.ViewModels;
using UrlShortener.DAL.Common.Interfaces;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.UnitOfWork.Interfaces;

namespace UrlShortener.BLL.Services.UrlService;

public class UrlService : IUrlService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Url, string> _urlRepository;

    private const int _pageSize = 5;
    private const int _numberOfCharsInShortUrl = 6;

    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghigklmnopqrstuvwxyz0123456789";
    private readonly Random _random = new();
    public UrlService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _urlRepository = _unitOfWork.GetRepository<Url, string>()!;
    }
    public async Task<string?> GetLongUrl(string id)
    {
        var url = await _urlRepository.GetByIdAsync(id);
        return url?.LongUrl;
    }
    public async Task<bool> CreateAsync(string url, int userId, string baseUrl)
    {
        if (_urlRepository
            .GetAllAsQueryable()
            .Any(u => u.LongUrl.Equals(url)))
            return false;

        var code = await GenerateUrlCodeAsync();
        var shortenUrl = new Url
        {
            Id = code,
            LongUrl = url,
            ShortUrl = baseUrl + code,
            CreatedAt = DateTime.Now,
            UserId = userId
        };
        await _urlRepository.CreateAsync(shortenUrl);
        await _unitOfWork.SaveAsync();

        return true;
    }
    public async Task<bool> DeleteAsync(string id)
    {
        var url = await _urlRepository.GetByIdAsync(id);

        if (url == null)
            return false;

        await _urlRepository.DeleteAsync(id);
        await _unitOfWork.SaveAsync();
        return true;
    }
    public async Task<Url?> GetUrl(string id)
    {
        return await _urlRepository.GetByIdAsync(id);
    }
    public async Task<UrlGridViewModel> GetPageAsync(int currentPage)
    {
        var allUrls = _urlRepository.GetAllAsQueryable();

        var urls = await allUrls
            .Skip((currentPage - 1) * _pageSize)
            .Take(_pageSize)
            .Select(url => new UrlViewModel
            {
                Id = url.Id,
                LongUrl = url.LongUrl,
                ShortUrl = url.ShortUrl,
                CreatedAt = url.CreatedAt,
                UserId = url.UserId,
                UserName = url.User.UserName!
            })
            .ToListAsync();

        return new UrlGridViewModel
        {
            Items = urls,
            TotalCount = allUrls.Count()
        };
    }
    private async Task<string> GenerateUrlCodeAsync()
    {
        var chars = new char[_numberOfCharsInShortUrl];
        while (true)
        {
            for (var i = 0; i < _numberOfCharsInShortUrl; i++)
            {
                var randomIndex = _random.Next(Alphabet.Length - 1);
                chars[i] = Alphabet[randomIndex];
            }

            var str = new string(chars);
            var url = await _urlRepository.GetByIdAsync(str);

            if (url == null)
                return str;
        }
    }
}