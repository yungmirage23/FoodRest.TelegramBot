using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;
using TelegramBot.Entities;
using TelegramBot.Services;

namespace TelegramBot.Commands
{
    public class StartCommand:BaseCommand
    {
        private readonly TelegramBotClient botClient;
        private readonly IUserService userService;
        private readonly PhoneConfirmationService phoneConfirmationService;
        public StartCommand(Bot telegramBot, IUserService _userService, PhoneConfirmationService _phoneConfirmationService)
        {
            botClient= telegramBot.GetBot().Result;  
            userService=_userService;
            phoneConfirmationService=_phoneConfirmationService;
        }
        public override string Name => CommandNames.StartCommand;

        public async override Task ExecuteAsync(Update update)
        {
            var user = await userService.GetOrCreate(update);
            if (user.PhoneNumber==null)
            {
                var inlineKeyboard = new ReplyKeyboardMarkup(new[]
                {
                    new[]
                    {
                        KeyboardButton.WithRequestContact("Ввести свой номер телефона")
                    }
                });
                inlineKeyboard.ResizeKeyboard = true;
                inlineKeyboard.OneTimeKeyboard = true;
                await botClient.SendTextMessageAsync(user.Id, $"Добро пожаловать ,{user.FirstName}!", ParseMode.Markdown, replyMarkup: inlineKeyboard);
            }
            else
            {
                await botClient.SendTextMessageAsync(user.Id, $"Вы уже успешно привязали свой номер телефона!({user.PhoneNumber})", ParseMode.Markdown);
                phoneConfirmationService.CreateConfirmationCode(user.PhoneNumber);
                await botClient.SendTextMessageAsync(user.Id, $"Ваш код подтверждения:", ParseMode.Markdown);
                await botClient.SendTextMessageAsync(user.Id, $"{phoneConfirmationService.GetConfirmationCode(user.PhoneNumber)}", ParseMode.Markdown);
            }
                
        }
    }
}
