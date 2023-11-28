namespace website.Services;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Microsoft.Extensions.Options;
using Lithium.Website;

public class MenuRepository : IMenuRepository
{
    private readonly WebsiteConfiguration websiteConfiguration;
    private readonly IDeserializer deserializer;
    private readonly IFileSystem fileSystem;

    public MenuRepository(IOptions<WebsiteConfiguration> websiteConfigurationOptions, IFileSystem fileSystem)
    {
        this.websiteConfiguration = websiteConfigurationOptions.Value;
        this.fileSystem = fileSystem;
        this.deserializer = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
    }

    public async Task<MenuItem[]> GetMenuItemsAsync() =>
        await Task.Run(() =>
        {
            var menuFilePath = $"{Path.Combine(websiteConfiguration.DataPath, "menu")}.yaml";
            using var reader = fileSystem.OpenText(menuFilePath);
            return deserializer.Deserialize<MenuItem[]>(reader);
        });
}
