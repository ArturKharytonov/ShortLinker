using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.BLL.Services.UrlService.Interfaces;
using UrlShortener.BLL.Services.UserContext.Interfaces;
using UrlShortener.BLL.ViewModels;

namespace UrlShortener.Controllers;

[Route("url")]
[ApiController]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;
    private readonly IUserContext _userContext;
    public UrlController(IUrlService urlService, IUserContext userContext)
    {
        _urlService = urlService;
        _userContext = userContext;
    }

    [HttpPost("adding")]
    [Authorize]
    public async Task<IActionResult> AddUrlAsync([FromBody] string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            return BadRequest("Wrong format of url");
        
        int.TryParse(_userContext.GetUserId(), out var userId);

        if (!await _urlService.CreateAsync(url, userId, GetBaseUrl()))
            return BadRequest("Url already exist");

        return Ok("Url was added");
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> NavigateToLongUrlAsync(string code)
    {
        var longUrl = await _urlService.GetLongUrl(code);

        if (string.IsNullOrEmpty(longUrl))
            return NotFound();
        
        return Redirect(longUrl);
    }


    [HttpDelete("{code}")]
    [Authorize]
    public async Task<IActionResult> DeleteAsync(string code)
    {
        if (await _urlService.DeleteAsync(code))
            return Ok("Url was deleted");

        return NotFound("Url was not found");
    }

    [HttpGet("info/{code}")]
    [Authorize]
    public async Task<IActionResult> GetUrlInfo(string code)
    {
        var url = await _urlService.GetUrl(code);

        if(url == null)
            return NotFound("Url was not found");

        return Ok(new UrlViewModel
        {
            Id = url.Id,
            LongUrl = url.LongUrl,
            ShortUrl = url.ShortUrl,
            CreatedAt = url.CreatedAt,
            UserId = url.UserId,
            UserName = url.User.UserName!
        });
    }

    [HttpGet("page")]
    public async Task<IActionResult> GetUrlPage(int page)
    {
        return Ok(await _urlService.GetPageAsync(page));
    }
    private string GetBaseUrl()
    {
        return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/url/";
    }
}