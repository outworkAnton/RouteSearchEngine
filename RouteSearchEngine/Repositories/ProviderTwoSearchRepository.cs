using System.Text;
using System.Text.Json;
using RouteSearchEngine.Abstractions;
using RouteSearchEngine.Entities.ProviderTwo;
using RouteSearchEngine.Models;
using Route = RouteSearchEngine.Models.Route;

namespace RouteSearchEngine.Repositories;

public class ProviderTwoSearchRepository : ISearchRepository
{
    private static readonly HttpClient client = new()
    {
        BaseAddress = new Uri("http://provider-two/api/v1"),
    };

    public async IAsyncEnumerable<Route> SearchRoutes(SearchRequest searchRequest, CancellationToken cancellationToken)
    {
        var request = new ProviderTwoSearchRequest
        {
            Arrival = searchRequest.Origin,
            Departure = searchRequest.Destination,
            DepartureDate = searchRequest.Filters.DestinationDateTime.GetValueOrDefault(),
            MinTimeLimit = searchRequest.Filters?.MinTimeLimit
        };

        using StringContent jsonContent = new(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        using var response = await client.PostAsync("search", jsonContent, cancellationToken);
        var responseRoutes = await response.Content.ReadFromJsonAsync<ProviderTwoSearchResponse>(cancellationToken: cancellationToken);

        foreach (var route in responseRoutes.Routes)
        {
            yield return new Route
            {
                Id = Guid.NewGuid(),
                Origin = route.Arrival.Point,
                Destination = route.Departure.Point,
                OriginDateTime = route.Arrival.Date,
                DestinationDateTime = route.Departure.Date,
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