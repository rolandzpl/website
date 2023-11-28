using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;

namespace Lithium.Website;

public class MyTransformer : DynamicRouteValueTransformer
{
    private readonly WebsiteConfiguration websiteConfiguration;
    private readonly IWebHostEnvironment environment;
    private readonly IActionDescriptorCollectionProvider actionDescriptorCollectionProvider;

    public MyTransformer(IOptions<WebsiteConfiguration> websiteConfigurationOptions, IWebHostEnvironment environment, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
        this.websiteConfiguration = websiteConfigurationOptions.Value;
        this.environment = environment;
        this.actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
    }

    public override async ValueTask<RouteValueDictionary> TransformAsync(HttpContext httpContext, RouteValueDictionary values)
    {
        return await Task.Run(() =>
        {
            if (string.IsNullOrWhiteSpace(values["slug"] as string))
            {
                values["slug"] = websiteConfiguration.StartPage;
            }
            values["page"] = "/DynamicPage";
            return values;
        });
    }
}