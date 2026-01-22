using Microsoft.AspNetCore.Mvc; 
using LocalMessenger.Services;
using LocalMessenger.Models;

[ApiController]
[Route("api/ai")]

public class AiController : ControllerBase
{
    private readonly OllamaServices _ai;
    public AiController(OllamaServices ai)
    {
        _ai = ai;
    }
    [HttpPost("generate")]
    public async Task<IActionResult> Generate(
        [FromBody] AiRequest request)
    {
        try
        {
            var result = await _ai.GenerateAsync(request.Prompt);
            return Ok( new{text = result});
        }
        catch(TimeoutException)
        {
            return StatusCode(504,"AI generation to long");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

