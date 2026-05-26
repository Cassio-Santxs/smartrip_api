using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace SmartTrip.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase {
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public ChatController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost]
    public async Task<ActionResult> Enviar([FromBody] DTOs.ChatRequest request)
    {
        var apiKey = _configuration["Gemini:ApiKey"];

        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}";

        var systemPrompt = @"Você é um assistente de turismo especializado em Foz do Iguaçu, Brasil. 
        Seu nome é SmarTrip. Você ajuda turistas a planejar roteiros personalizados, 
        sugerindo atrações como Cataratas do Iguaçu, Usina de Itaipu, Parque das Aves, 
        Tríplice Fronteira e outros pontos turísticos da região. 
        Responda sempre em português, de forma amigável e objetiva.
        Quando sugerir roteiros, organize por dia e horário.";

        var body = new
        {
            systemInstruction = new
            {
                parts = new[]
                {
                new { text = systemPrompt }
            }
            },
            contents = new[]
            {
            new
            {
                parts = new[]
                {
                    new { text = request.Mensagem }
                }
            }
        }
        };

        var json = JsonSerializer.Serialize(body);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode) {
            var errorDetails = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, $"Erro ao comunicar com o Gemini: {errorDetails}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(responseBody);

        var texto = jsonDoc
            .RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return Ok(new { resposta = texto });
    }
}