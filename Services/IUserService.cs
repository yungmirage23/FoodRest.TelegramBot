using Telegram.Bot.Types;
using TelegramBot.Commands;
using TelegramBot.Entities;

namespace TelegramBot.Services
{
    public interface IUserService
    {
        Task<AppUser> GetOrCreate(Update update);
        Task<AppUser> SetUserPhone(long chatId,string cachePhoneNumber);
        Task<AppUser> UserMove(AppUser user, string commandName);
    }
}
