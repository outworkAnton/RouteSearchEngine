using RouteSearchEngine.Abstractions;
using RouteSearchEngine.Models;
using Route = RouteSearchEngine.Models.Route;

namespace RouteSearchEngine.Services;

public class SearchService(IEnumerable<ISearchRepository> repositories, ICacheService cacheService) : ISearchService
{
    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var routes = new List<Route>();

        if (request.Filters?.OnlyCached is true)
        {
            routes = cacheService.GetRoutes().ToList();
        }
        else
        {
            foreach (var repository in repositories)
            {
                if (await repository.IfAvailable(cancellationToken))
                {
                    routes.AddRange(await repository.SearchRoutes(request, cancellationToken).ToArrayAsync(cancellationToken: cancellationToken));
                }
            }

            if (routes.Count == 0)
            {
                routes = cacheService.GetRoutes().ToList();
            }
            else
            {
                cacheService.ClearCache();
                cacheService.CacheRoutes(routes);
            }
        }

        return new SearchResponse
        {
            Routes = routes.ToArray(),
            MaxPrice = routes.Max(route => route.Price),
            MinPrice = routes.Min(route => route.Price),
            MaxMinutesRoute = (int)routes.Max(route => (route.DestinationDateTime - route.OriginDateTime).TotalMinutes),
            MinMinutesRoute = (int)routes.Min(route => (route.DestinationDateTime - route.OriginDateTime).TotalMinutes)
        };
    }
}