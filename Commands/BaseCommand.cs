using Telegram.Bot.Types;
using TelegramBot.Entities;

namespace TelegramBot.Commands
{
    public abstract class BaseCommand
    {
        public abstract string Name { get; }
        public abstract Task ExecuteAsync(Update update,AppUser user);
    }
}
