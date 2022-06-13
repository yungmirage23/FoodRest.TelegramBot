using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Entities;
using TelegramBot.Models.DataContext;
using TelegramBot.Services;

namespace TelegramBot.Controllers
{
    [ApiController]
    [Route("api/phone/confirm")]
    public class PhoneConfirmController:ControllerBase
    {
        private PhoneConfirmationService confirmationServiceservice;
        private readonly UserDataContext dataContext;
        private readonly TelegramBotClient telegramClient;
        public PhoneConfirmController(Bot _telegrambot,PhoneConfirmationService _confirmationService,UserDataContext _dataContext)
        {
            confirmationServiceservice= _confirmationService;
            dataContext= _dataContext;
            telegramClient = _telegrambot.GetBot().Result;
        }
        [HttpPost]
        public async Task<int> Post([FromBody]string phoneNumber)
        {
            var user =dataContext.Users.FirstOrDefault(p => p.PhoneNumber == phoneNumber);
            var code =confirmationServiceservice.CreateConfirmationCode(phoneNumber);
            if (user != null)
            {
                await telegramClient.SendTextMessageAsync(user.Id, "Ваш код подтверждения:", ParseMode.Markdown);
                await telegramClient.SendTextMessageAsync(user.Id, $"{code}", ParseMode.Markdown);
            }
            return code;
        }
    }
}
