namespace Lithium.Website.Domain;

public interface IPageService
{
    Task CreatePage(CreatePageRequest reque);
    Task AmendPage(AmendPageRequest request);

    Task PublishPage(PublishPageRequest request);

    Task UnpublishPage(UnpublishPageRequest request);

    Task<ValidationSlugResponse> ValidateSlug();
}
