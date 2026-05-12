using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Transit.Application.Features.Polaczenia.Queries.WyszukajPolaczenia;

namespace Transit.Api.IntegrationTests.Polaczenia;

[Collection("Integration")]
public class WyszukajPolaczeniaTests(CustomWebApplicationFactory factory)
{
    private readonly HttpClient _client = factory.Client;

    [Fact]
    public async Task Wyszukaj_ten_sam_przystanek_zwraca_400()
    {
        var response = await _client.GetAsync(
            "/api/v1/polaczenia?przystanekZ=1&przystanekDo=1&data=2026-05-13&czas=08:00");
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Wyszukaj_nieistniejace_przystanki_zwraca_pusta_liste()
    {
        var response = await _client.GetAsync(
            "/api/v1/polaczenia?przystanekZ=9999&przystanekDo=9998&data=2026-05-13&czas=08:00");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await response.Content.ReadFromJsonAsync<List<PolaczenieDto>>();
        list.Should().BeEmpty();
    }
}
