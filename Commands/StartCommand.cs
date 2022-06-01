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
        public StartCommand(Bot telegramBot, IUserService _userService)
        {
            botClient= telegramBot.GetBot().Result;  
            userService=_userService;
        }
        public override string Name => CommandNames.StartCommand;

        public async override Task ExecuteAsync(Update update)
        {
            var user = await userService.GetOrCreate(update);
            var inlineKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    KeyboardButton.WithRequestContact("Подтвердить свой номер телефона")
                }
            });
            inlineKeyboard.ResizeKeyboard = true;
            inlineKeyboard.OneTimeKeyboard = true;
            await botClient.SendTextMessageAsync(user.Id, $"Добро пожаловать ,{user.FirstName}!",ParseMode.Markdown,replyMarkup:inlineKeyboard);
        }
    }
}
