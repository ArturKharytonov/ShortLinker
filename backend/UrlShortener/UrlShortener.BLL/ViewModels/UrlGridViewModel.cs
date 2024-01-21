namespace UrlShortener.BLL.ViewModels;

public class UrlGridViewModel
{
    public IEnumerable<UrlViewModel> Items { get; set; }
    public int TotalCount { get; set; }
}