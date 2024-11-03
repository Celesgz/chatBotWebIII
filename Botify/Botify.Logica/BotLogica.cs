namespace Botify.Logica;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using Botify.Entidades;
using Microsoft.Extensions.Options;
using Botify.Entidades;
using System.Text.Json.Serialization;
using System.Text;

public interface IBotLogica
{
    public Task<string> ObtenerToken();
    Task<string> ObtenerRecomendaciones(string mood);

}
public class BotLogica : IBotLogica
{
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly HttpClient httpClient = new HttpClient();

    public BotLogica(IOptions<SpotifyConfig> options)
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

    public async Task<string> ObtenerRecomendaciones(string mood)
    {
        // Busca la clave (estado de ánimo) y el valor (género) correspondientes en el diccionario
        var moodGenrePair = MoodToGenreMap.FirstOrDefault(entry => mood.Contains(entry.Key, StringComparison.OrdinalIgnoreCase));
        var genre = moodGenrePair.Value;
        var moodKey = moodGenrePair.Key;

        if (!string.IsNullOrEmpty(genre))
        {
            var accessToken = await ObtenerToken();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync($"https://api.spotify.com/v1/recommendations?seed_genres={genre}&limit=3&market=AR");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            // Deserializar la respuesta
            var recommendationResponse = JsonSerializer.Deserialize<RecommendationResponse>(content, options);

            // Formatear la respuesta
            var formattedRecommendations = new StringBuilder();
            // Capitalizar la primera letra de moodKey
            formattedRecommendations.AppendLine($"Recomendaciones para el estado de ánimo: {char.ToUpper(moodKey[0]) + moodKey.Substring(1)}");

            foreach (var track in recommendationResponse.Tracks)
            {
                var artistNames = string.Join(", ", track.Artists.Select(a => a.Name));
                var albumImageUrl = track.Album.Images.FirstOrDefault()?.Url ?? "Imagen no disponible";
                formattedRecommendations.AppendLine($"- **{track.Name}** de {artistNames} (Album: {track.Album.Name}) - [Escuchar aquí]({track.ExternalUrls.Spotify}) ![Portada]({albumImageUrl})\n");
            }

            return formattedRecommendations.ToString();
        }
        else
        {
            return $"No hay recomendaciones disponibles para el estado de ánimo: {mood}.";
        }
    }


    private static readonly Dictionary<string, string> MoodToGenreMap = new Dictionary<string, string>
{
    { "feliz", "pop" },
    { "triste", "blues" },
    { "energético", "dance" },
    { "relajado", "jazz" },
    { "romántico", "romance" },
    // Agrega más estados y géneros según sea necesario
};

}

