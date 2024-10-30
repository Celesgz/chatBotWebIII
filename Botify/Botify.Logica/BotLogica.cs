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
    Task<string> SendMessageToBot(string message);
}
public class BotLogica : IBotLogica
{
    private readonly HttpClient _httpClient;
    private const string _botEndpoint = "http://localhost:3978/api/messages";

    public BotLogica(HttpClient httpClient)
    {
        _httpClient = httpClient;

    }

    public async Task<string> SendMessageToBot(string message)
    {
        var payload = new
        {
            type = "message",
            text = message
        };

        // Usar System.Text.Json
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_botEndpoint, content);

        if (response.IsSuccessStatusCode)
        {
            var botResponse = await response.Content.ReadAsStringAsync();
            return botResponse;
        }
        else
        {
            throw new HttpRequestException($"Error en la comunicación con el bot: {response.ReasonPhrase}");
        }
    }
}
