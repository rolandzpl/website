using Microsoft.AspNetCore.JsonPatch.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using website.Services;

namespace Lithium.Website.Controllers;

[ApiController]
[Route("[controller]")]
public class SEOController : ControllerBase
{
    private readonly WebsiteConfiguration websiteConfiguration;
    private readonly IFileSystem fileSystem;

    public SEOController(IOptions<WebsiteConfiguration> websiteConfigurationOptions, IFileSystem fileSystem)
    {
        this.websiteConfiguration = websiteConfigurationOptions.Value;
        this.fileSystem = fileSystem;
    }

    [HttpGet("/robots.txt")]
    public IActionResult GetRobotsFile() => GetResult("robots.txt");

    [HttpGet("/sitemap.xml")]
    public IActionResult GetSitemap() => GetResult("sitemap.xml");

    private IActionResult GetResult(string fileName) => base.File(OpenStream(fileName), GetContentType(fileName));

    private Stream OpenStream(string fileName) => fileSystem.OpenRead(GetFilePath(fileName));

    private string GetFilePath(string fileName) => Path.Combine(websiteConfiguration.DataPath, fileName);

    private string GetContentType(string fileName)
    {
        new FileExtensionContentTypeProvider().TryGetContentType(fileName, out string contentType);
        return contentType;
    }
}