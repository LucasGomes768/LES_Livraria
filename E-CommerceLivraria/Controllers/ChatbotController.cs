using E_CommerceLivraria.DTO.ChatbotDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using RestSharp;

namespace E_CommerceLivraria.Controllers
{
    public class ChatbotController : Controller
    {
        [HttpPost("/Chatbot/SendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] UserMessageDTO userMessageJson)
        {
            try
            {
                if (userMessageJson == null)
                {
                    return BadRequest("Valor foi enviado nulo");
                }

                var client = new RestClient("http://localhost:5000/gemini/send");

                var jsonString = JsonSerializer.Serialize(userMessageJson, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var request = new RestRequest("", Method.Post);
                request.AddParameter("application/json", jsonString, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                var response = await client.PostAsync(request);

                if (response == null)
                {
                    return NotFound();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(new
                    {
                        Sucess = true,
                        response.Content
                    });
                }
                else
                {
                    int code = (int)response.StatusCode;
                    return StatusCode(code, new
                    {
                        Sucess = false,
                        response.Content
                    });
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                return StatusCode(500, new
                {
                    Sucess = false,
                    ex.Message
                });
            }
        }
    }
}
