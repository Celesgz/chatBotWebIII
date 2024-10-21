namespace API.Spotify.Logica;

using System;
using System.Net.Http;
using System.Threading.Tasks;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using API.Spotify.Entidades;
using Microsoft.Extensions.Options;

public interface ITokenLogica
{
    public Task<string> ObtenerToken();

    public Task<string> ObtenerInformacionDelArtista(string artistaId);

}
public class TokenLogica : ITokenLogica
{
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly HttpClient httpClient = new HttpClient();

    public TokenLogica(IOptions<SpotifyConfig> options)
    {
        clientId = options.Value.ClientId;
        clientSecret = options.Value.ClientSecret;
    }

    public async Task<string> ObtenerToken()
    {
        var authToken = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));

        var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();

        var tokenResponse = JsonSerializer.Deserialize<SpotifyToken>(responseContent);

        return tokenResponse.access_token;
    }

    public async Task<string> ObtenerInformacionDelArtista(string artistId)
    {
        var accessToken = await ObtenerToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await httpClient.GetAsync($"https://api.spotify.com/v1/artists/{artistId}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return content; 
    }


}

