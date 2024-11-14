using System.Text;
using System.Text.Json;
using RouteSearchEngine.Abstractions;
using RouteSearchEngine.Entities.ProviderOne;
using RouteSearchEngine.Models;
using Route = RouteSearchEngine.Models.Route;

namespace RouteSearchEngine.Repositories;

public class ProviderOneSearchRepository : ISearchRepository
{
    private static readonly HttpClient client = new()
    {
        BaseAddress = new Uri("http://provider-one/api/v1"),
    };

    public async IAsyncEnumerable<Route> SearchRoutes(SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var request = new ProviderOneSearchRequest
        {
            From = searchRequest.Origin,
            To = searchRequest.Destination,
            DateFrom = searchRequest.OriginDateTime,
            DateTo = searchRequest.Filters?.DestinationDateTime,
            MaxPrice = searchRequest.Filters?.MaxPrice
        };

        using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        using var response = await client.PostAsync("search", jsonContent, cancellationToken);
        var responseRoutes = await response.Content.ReadFromJsonAsync<ProviderOneSearchResponse>(cancellationToken: cancellationToken);

        foreach (var route in responseRoutes.Routes)
        {
            yield return new Route
            {
                Id = Guid.NewGuid(),
                Origin = route.From,
                Destination = route.To,
                OriginDateTime = route.DateFrom,
                DestinationDateTime = route.DateTo,
                Price = route.Price,
                TimeLimit = route.TimeLimit
            };
        }
    }

    public async Task<bool> IfAvailable(CancellationToken cancellationToken)
    {
        using var response = await client.GetAsync("ping", cancellationToken);

        return response.IsSuccessStatusCode;
    }
}