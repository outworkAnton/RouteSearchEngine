using RouteSearchEngine.Models;
using Route = RouteSearchEngine.Models.Route;

namespace RouteSearchEngine.Abstractions;

public interface ISearchRepository
{
    IAsyncEnumerable<Route> SearchRoutes(SearchRequest searchRequest, CancellationToken cancellationToken);

    Task<bool> IfAvailable(CancellationToken cancellationToken);
}