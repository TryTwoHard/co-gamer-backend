using Microsoft.AspNetCore.Mvc.Testing;
using Tournament.API.Controllers;
using Xunit;

namespace Tournament.API.Tests.Integration;

public class TournamentsControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>
{
    private readonly HttpClient _httpClient;

    public TournamentsControllerTests(WebApplicationFactory<IApiMarker> appFactory)
    {
        _httpClient = appFactory.CreateClient();
    }
}