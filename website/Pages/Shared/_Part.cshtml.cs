namespace website.Pages;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.RazorPages;
using website.Services;
using Lithium.Website;

public class PartModel : PageModel
{
    private readonly WebsiteConfiguration websiteConfiguration;
    private readonly IFileSystem fileSystem;

    public string Content { get; private set; }

    public PartModel(IOptions<WebsiteConfiguration> websiteConfigurationOptions, IFileSystem fileSystem)
    {
        this.websiteConfiguration = websiteConfigurationOptions.Value;
        this.fileSystem = fileSystem;
    }

    public async Task OnGet(string name)
    {
        var partFilePath = Path.ChangeExtension(Path.Join(websiteConfiguration.DataPath, name), "html");
        Content = fileSystem.Exists(partFilePath)
            ? await fileSystem.ReadAllTextAsync(partFilePath)
            : "???";
    }
}
