namespace UrlShortener.BLL.ViewModels;

public class UrlViewModel
{
    public string Id { get; set; }
    public string LongUrl { get; set; } = null!;
    public string ShortUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
}