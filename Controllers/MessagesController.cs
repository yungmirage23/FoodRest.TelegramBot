using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBot.Entities;
using TelegramBot.Models;
using TelegramBot.Services;

namespace TelegramBot.Controllers
{
    [ApiController]
    [Route("api/message/update")]
    public class MessagesController : ControllerBase
    {
        private readonly ICommandExecutor commandExecutor;
        public MessagesController(ICommandExecutor _commandExecutor)
        {
            commandExecutor=_commandExecutor;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Update update)
        {
            if (update?.Message?.Chat== null && update?.CallbackQuery==null) return Ok();
            try
            {
                await commandExecutor.Execute(update);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }
    }
}