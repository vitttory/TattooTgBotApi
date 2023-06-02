using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Http.Headers;

namespace TattooTgBotApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class CreateSketchController : Controller
    {
        [HttpGet("/index/{prompt}")]
        public async Task<string> Get(string prompt)
        {
            prompt = prompt.Replace('_', ' ');
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.openai.com/v1/images/generations"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", "Bearer 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");

                    request.Content = new StringContent("{\n    \"prompt\": \"a tattoo sketch of " + prompt.Replace('_', ' ') + "\",\n    \"n\": 1,\n    \"size\": \"512x512\"\n  }");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    var answer = await response.Content.ReadAsStringAsync();
                    return answer;
                }
            }
        }
    }
}

