using Telegram.Bot.Types;

namespace TelegramBot.Services
{
    public interface ICommandExecutor
    {
        Task Execute(Update update);
    }
}
