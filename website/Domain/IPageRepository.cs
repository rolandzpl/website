namespace Lithium.Website.Domain;

public interface IPageRepository
{
    Task<PageListResultDto> GetAllPages();
}

public record PageShortResultDto(string Slug);

public record PageListResultDto(PageShortResultDto[] Pages);
