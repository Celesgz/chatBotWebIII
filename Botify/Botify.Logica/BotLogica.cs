using Botify.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Botify.Logica;

public interface IBotLogica
{
    Task<Artista> RecomendarArtista();
}
public class BotLogica : IBotLogica
{
    private readonly HttpClient _httpClient;
    private readonly ITokenLogica _tokenLogica;

    public BotLogica(HttpClient httpClient, ITokenLogica tokenLogica)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _tokenLogica = tokenLogica;
    }

    public async Task<Artista> RecomendarArtista()
    {
        var accessToken = await _tokenLogica.ObtenerToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/artists/4dpARuHxo51G3z768sgnrY");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var artista = JsonSerializer.Deserialize<Artista>(content, options);

        return artista;
    }

}
