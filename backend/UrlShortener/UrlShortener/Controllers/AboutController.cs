using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace UrlShortener.Controllers;

public class AboutController : Controller
{
    private const string JsonFilePath = "aboutContent.json";
    public IActionResult Index()
    {
        var content = "test";
        return View("Index", content);
    }

    [HttpPost]
    public IActionResult SaveContent(string editedContent)
    {
        SaveContentToJson(editedContent);
        return RedirectToAction("Index");
    }

    private void SaveContentToJson(string content)
    {
        var serializedContent = JsonSerializer.Serialize(content);
        System.IO.File.WriteAllText(JsonFilePath, serializedContent);
    }

    private string ReadContentFromJson()
    {
        if (!System.IO.File.Exists(JsonFilePath)) 
            return string.Empty;
        var serializedContent = System.IO.File.ReadAllText(JsonFilePath);
        return JsonSerializer.Deserialize<string>(serializedContent);
    }
}