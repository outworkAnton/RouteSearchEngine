using Route = RouteSearchEngine.Models.Route;

namespace RouteSearchEngine.Abstractions;

public interface ICacheService
{
    public void CacheRoutes(IReadOnlyCollection<Route> routes);

    public IReadOnlyCollection<Route> GetRoutes();
    void ClearCache();
}