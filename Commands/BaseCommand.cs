using Telegram.Bot.Types;

namespace TelegramBot.Commands
{
    public abstract class BaseCommand
    {
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(Update update);
    }
}
