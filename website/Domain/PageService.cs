namespace Lithium.Website.Domain;

public class PageService : IPageService
{
    public async Task CreatePage(CreatePageRequest request)
    {
        WebsitePage.Create();
    }

    public async Task AmendPage(AmendPageRequest request) { }

    public async Task PublishPage(PublishPageRequest request) { }

    public async Task UnpublishPage(UnpublishPageRequest request) {}

    public async Task<ValidationSlugResponse> ValidateSlug() => new ValidationSlugResponse();
}
