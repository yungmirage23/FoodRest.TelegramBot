using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBot.Entities;
using TelegramBot.Models;
using TelegramBot.Services;

namespace TelegramBot.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "SUCCSESS SUKAAAA";
        }
    }
}