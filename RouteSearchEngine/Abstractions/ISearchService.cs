using RouteSearchEngine.Models;

namespace RouteSearchEngine.Abstractions;

public interface ISearchService
{
    Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
}