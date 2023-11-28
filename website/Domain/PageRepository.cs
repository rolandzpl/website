using Microsoft.Extensions.Options;
using website.Services;
using static Lithium.Website.Domain.IPageRepository;

namespace Lithium.Website.Domain;

public class PageRepository : IPageRepository
{
    private readonly WebsiteConfiguration websiteConfiguration;
    private readonly IFileSystem fileSystem;
    private readonly ILogger<PageRepository> logger;

    public PageRepository(IOptions<WebsiteConfiguration> websiteConfigurationOptions, IFileSystem fileSystem, ILogger<PageRepository> logger)
        : this(websiteConfigurationOptions.Value, fileSystem, logger) { }

    public PageRepository(WebsiteConfiguration websiteConfiguration, IFileSystem fileSystem, ILogger<PageRepository> logger)
    {
        this.fileSystem = fileSystem;
        this.logger = logger;
        this.websiteConfiguration = websiteConfiguration;
    }

    public async Task<PageListResultDto> GetAllPages() =>
        new PageListResultDto(fileSystem
            .GetFiles(websiteConfiguration.DataPath)
            .Where(IsHtml)
            .Select(OnlySlugPart)
            .Select(path => new PageShortResultDto(path))
            .ToArray());

    private string OnlySlugPart(string path) => fileSystem.GetFileNameWithoutExtension(path);

    private bool IsHtml(string path) => fileSystem.GetExtension(path) == ".html";
}
