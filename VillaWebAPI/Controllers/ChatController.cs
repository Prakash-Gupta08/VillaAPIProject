using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using VillaWebAPI.Services;

namespace VillaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Chat(ChatRequestDto request)
        {
            var reply = await _chatService.GetReply(request.Message);

            return Ok(new ChatResponseDto
            {
                Reply = reply
            });
        }
    }
}
