using RouteSearchEngine.Abstractions;
using Route = RouteSearchEngine.Models.Route;

namespace RouteSearchEngine.Services;

public class CacheService : ICacheService
{
    private readonly Dictionary<Guid, Route> _memoryCache;

    public void CacheRoutes(IReadOnlyCollection<Route> routes)
    {
        foreach (var route in routes)
        {
            _memoryCache.TryAdd(route.Id, route);
        }
    }

    public IReadOnlyCollection<Route> GetRoutes()
    {
        return _memoryCache.Values;
    }

    public void ClearCache()
    {
        _memoryCache.Clear();
    }
}