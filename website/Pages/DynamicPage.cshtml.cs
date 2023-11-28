namespace website.Pages;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.RazorPages;
using website.Services;
using Lithium.Website;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

public class DynamicPageModel : PageModel
{
    private readonly WebsiteConfiguration websiteConfiguration;
    private readonly IFileSystem fileSystem;
    private readonly ILogger<DynamicPageModel> logger;
    private readonly IDeserializer deserializer;

    public string CanonicalUrl { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Dictionary<string, string> MetaTags { get; } = new Dictionary<string, string>();
    public string Slug { get; private set; }
    public string Content { get; private set; }
    public string Script { get; private set; }

    public DynamicPageModel(IOptions<WebsiteConfiguration> websiteConfigurationOptions, IFileSystem fileSystem, ILogger<DynamicPageModel> logger)
    {
        this.websiteConfiguration = websiteConfigurationOptions.Value;
        this.fileSystem = fileSystem;
        this.logger = logger;
        this.deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
    }

    public async Task<IActionResult> OnGet()
    {
        Slug = RouteData.Values["slug"] as string;
        logger.LogInformation("Page slug: {slug}", Slug);
        var pageFilePath = Path.ChangeExtension(Path.Join(websiteConfiguration.DataPath, Slug), "html");
        if (!fileSystem.Exists(pageFilePath))
        {
            return NotFound();
        }
        logger.LogInformation("Page content is located in {path}", pageFilePath);
        Content = await fileSystem.ReadAllTextAsync(pageFilePath);
        var scriptFilePath = Path.ChangeExtension(Path.Join(websiteConfiguration.DataPath, Slug), "js");
        if (fileSystem.Exists(scriptFilePath))
        {
            Script = await fileSystem.ReadAllTextAsync(scriptFilePath);
            logger.LogInformation("Attaching script file: {scriptFilePath}", scriptFilePath);
        }
        try
        {
            var metadataFilePath = Path.ChangeExtension(Path.Join(websiteConfiguration.DataPath, Slug), "yml");
            if (fileSystem.Exists(metadataFilePath))
            {
                logger.LogInformation("Parsing page metadata from: {metadataFilePath}", metadataFilePath);
                using var metadataReader = fileSystem.OpenText(metadataFilePath);
                var metadata = deserializer.Deserialize<PageMetadata>(metadataReader);
                if (metadata.Published == false)
                {
                    return NotFound();
                }
                ViewData["Title"] = Title = metadata.Title;
                Description = metadata.Description;
                if (metadata.MetaTags.Any())
                {
                    var additionalMetaTags = string.Join(", ", metadata.MetaTags.Keys);
                    logger.LogInformation("Additional meta tags in page:{additionalMetaTags}", additionalMetaTags);
                    foreach (var item in metadata.MetaTags.AsEnumerable())
                    {
                        if (!MetaTags.TryAdd(item.Key, item.Value))
                        {
                            logger.LogError("Failed adding meta tag {key}", item.Key);
                        }
                    }
                }
            }
            return Page();
        }
        catch (Exception ex)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    public class PageMetadata
    {
        public bool? Published { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Dictionary<string, string> MetaTags { get; set; } = new Dictionary<string, string>();
    }
}