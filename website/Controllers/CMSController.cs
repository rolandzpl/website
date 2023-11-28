using Lithium.Website.Domain;
using Lithium.Website.UseCases;
using Microsoft.AspNetCore.Mvc;
using static Lithium.Website.Domain.IPageRepository;

namespace Lithium.Website.Controllers;

[ApiController]
[Route("[controller]")]
public class CMSController : ControllerBase
{
    private readonly IPageRepository pageRepository;
    private readonly CreateNewPage createNewPage;
    private readonly AmendPage  amendPage;

    public CMSController(IPageRepository pageRepository, CreateNewPage createNewPage, AmendPage amendPage)
    {
        this.pageRepository = pageRepository;
        this.createNewPage = createNewPage;
        this.amendPage = amendPage;
    }

    [HttpGet("/api/cms/pages")]
    public async Task<PageListResultDto> GetAllPages() => await pageRepository.GetAllPages();

    [HttpPost("/api/cms/pages/{slug}")]
    public async Task CreateNewPage() => await createNewPage.ExecuteAsync();

    [HttpPut("/api/cms/pages/{slug}")]
    public async Task<PageListResultDto> AmendPage() => await amendPage.ExecuteAsync();
}
