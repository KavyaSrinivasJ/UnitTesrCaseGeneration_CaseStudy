using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Chat;
using Microsoft.AspNetCore.Cors;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TestCaseGenerator.Controllers
{
    [EnableCors("MyPolicy")]
    [ApiController]
    public class OpenAIController : Controller
    {
        private readonly string _openAiApiKey = "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";


        /// <summary>
        /// API to generate unit test cases by code snippet
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("/Code")]
        public async Task<IActionResult> GetUnitTestByCodeAsync([FromForm] string code)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(code))
                {
                    return BadRequest("Code cannot be empty or whitespace.");
                }

                // Initialize OpenAI API client
                var openai = new OpenAIAPI(_openAiApiKey);
                string defaultText = "Generate maximum unit test code for this program";
                var promptText = defaultText + code;

                var chatMessageList = new List<ChatMessage>
        {
            new ChatMessage {Role = ChatMessageRole.User, TextContent = promptText },
        };

                // Prepare completion request
                var completion = new ChatRequest
                {
                    Temperature = 0.5,
                    Messages = chatMessageList,
                    Model = OpenAI_API.Models.Model.ChatGPTTurbo,
                    MaxTokens = 4000
                };

                // Request completion asynchronously
                var result = await openai.Chat.CreateChatCompletionAsync(completion);

                // Check if completions are available
                if (result?.Choices?.Any() ?? false)
                {
                    // Get the text of the first completion
                    var answer = result.Choices.First().Message.TextContent;
                    return Ok(answer);
                }
                else
                {
                    return BadRequest("No completions found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString(), "An error occurred while generating unit tests.");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}

