using Gig.Platform.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gig_Platform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        private IActionResult HandleError(IEnumerable<string> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error);
            }
            return BadRequest(ModelState.Values);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageRequestDto messageRequestDto)
        {
            var result = await _messageService.CreateAsync(messageRequestDto.Content, messageRequestDto.SenderId, messageRequestDto.ReceiverId);
            if (result.IsSucces)
            {
                return CreatedAtAction(nameof(Create), new { result.Value.Id }, new MessageResponseDto
                {
                    Id = result.Value.Id,
                    Content = result.Value.Content,
                    SenderId = result.Value.SenderId,
                    ReceiverId = result.Value.ReceiverId,
                    Created = result.Value.Created
                });
            }
            return HandleError(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, MessageRequestDto messageRequestDto)
        {
            var result = await _messageService.UpdateAsync(id, messageRequestDto.Content);
            if (result.IsSucces)
            {
                return Ok(new MessageResponseDto
                {
                    Id = result.Value.Id,
                    Content = result.Value.Content,
                    SenderId = result.Value.SenderId,
                    ReceiverId = result.Value.ReceiverId,
                    Created = result.Value.Created
                });
            }
            return HandleError(result.Errors);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _messageService.DeleteAsync(id);
            if (result.IsSucces)
            {
                return Ok("Message deleted!");
            }
            return HandleError(result.Errors);
        }

        [HttpGet("conversation/{id1}/{id2}")]
        public async Task<IActionResult> GetConversation(Guid id1, Guid id2)
        {
            var result = await _messageService.GetConversationAsync(id1, id2);
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(x => new MessageResponseDto
                {
                    Id = x.Id,
                    Content = x.Content,
                    SenderId = x.SenderId,
                    ReceiverId = x.ReceiverId,
                    Created = x.Created
                }));
            }
            return HandleError(result.Errors);
        }

        [HttpGet("partners/{id}")]
        public async Task<IActionResult> GetPartners(Guid id)
        {
            var result = await _messageService.GetAllConversationPartnersAsync(id);
            if (result.IsSucces)
            {
                return Ok(result.Value.Select(x => new AccountResponseDto
                {
                    Id = x.Id,
                    UserName = x.UserName
                }));
            }
            return HandleError(result.Errors);
        }
    }
}
