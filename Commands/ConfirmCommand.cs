using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;
using TelegramBot.Entities;
using TelegramBot.Services;

namespace TelegramBot.Commands
{
    public class ConfirmCommand:BaseCommand
    {
        private readonly PhoneConfirmationService phoneConfirmation;
        private readonly TelegramBotClient botClient;
        private readonly IUserService userService;

        public ConfirmCommand(Bot telegrambot,IUserService _userService, PhoneConfirmationService _phoneConfirmation)
        {
            userService = _userService;
            botClient = telegrambot.GetBot().Result;
            phoneConfirmation = _phoneConfirmation;
        }
        public override string Name => CommandNames.ConfirmCommand;

        public async override Task ExecuteAsync(Update update)
        {
            var user=await userService.GetOrCreate(update);
            if (await phoneConfirmation.SetPhoneFromCache(update))
            {
                await botClient.SendTextMessageAsync(user.Id, "Номер телефона успешно зарегистрирован, ожидайте код подтверждения",ParseMode.Markdown, replyMarkup: new ReplyKeyboardRemove());
                return;
            } 
            await botClient.SendTextMessageAsync(user.Id, "Произошла ошибка при регистрации номера телефона",ParseMode.Markdown, replyMarkup:new ReplyKeyboardRemove());

        }

    }
}
