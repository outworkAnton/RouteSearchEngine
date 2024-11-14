using Microsoft.AspNetCore.Mvc;
using RouteSearchEngine.Abstractions;
using RouteSearchEngine.Models;

namespace RouteSearchEngine.Controllers;

[ApiController]
[Route("search")]
public class SearchController(ISearchService searchService) : ControllerBase
{
    [HttpPost]
    public async Task<SearchResponse> Search([FromBody] SearchRequest request, CancellationToken token) =>
        await searchService.SearchAsync(request, token);
}