using Microsoft.Extensions.Options;
using Lithium.Website;
using website.Services;
using System.Globalization;

class PageExistsRouteConstraint : IRouteConstraint
{
    private readonly WebsiteConfiguration websiteConfiguration;
    private readonly IFileSystem fileSystem;

    public PageExistsRouteConstraint(IOptions<WebsiteConfiguration> websiteConfigurationOptions, IFileSystem fileSystem)
    {
        this.websiteConfiguration = websiteConfigurationOptions.Value;
        this.fileSystem = fileSystem;
    }

    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.TryGetValue(routeKey, out var routeValue))
        {
            return false;
        }

        var routeValueString = Convert.ToString(routeValue, CultureInfo.InvariantCulture);

        if (routeValueString is null)
        {
            return false;
        }

        if (string.IsNullOrEmpty(routeValueString))
        {
            return true;
        }

        var pageFilePath = Path.ChangeExtension(Path.Join(websiteConfiguration.DataPath, routeValueString), "html");
        return fileSystem.Exists(pageFilePath);
    }
}
