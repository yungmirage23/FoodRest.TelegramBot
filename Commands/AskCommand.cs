using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Models;
using TelegramBot.Services;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Entities;

namespace TelegramBot.Commands
{
    public class AskCommand : BaseCommand
    {
        private readonly IUserService userService;
        private readonly TelegramBotClient botClient;
        private PhoneConfirmationService confirmationService;
        public AskCommand(IUserService _userService, Bot telegramBot, PhoneConfirmationService _confirmationService)
        {
            userService= _userService;
            botClient= telegramBot.GetBot().Result;
            confirmationService= _confirmationService;
        }
        public override string Name => CommandNames.AskCommand;

        public override async Task ExecuteAsync(Update update)
        {
            var user = await userService.GetOrCreate(update);
            confirmationService.SavePhoneInCache(user.Id,update.Message.Contact.PhoneNumber);
            var inlineKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Да"),
                    new KeyboardButton("Нет"), 
                }
            });
            inlineKeyboard.ResizeKeyboard = true;
            inlineKeyboard.OneTimeKeyboard = true;
            await botClient.SendTextMessageAsync(user.Id, $"Ваш номер телефона - {update.Message.Contact.PhoneNumber} ?",ParseMode.Markdown,replyMarkup:inlineKeyboard);
        }
    }
}
