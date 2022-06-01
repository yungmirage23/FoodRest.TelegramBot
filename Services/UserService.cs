using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Entities;
using TelegramBot.Models.DataContext;

namespace TelegramBot.Services
{
    public class UserService:IUserService
    {
        private readonly UserDataContext dataContext;

        public UserService(UserDataContext _dataContext )
        {
            dataContext= _dataContext;
        }

        public async Task<AppUser> GetOrCreate(Update update)
        {
            var newUser = update.Type switch
            {
                UpdateType.CallbackQuery => new AppUser
                {
                    UserName = update.CallbackQuery.From.Username,
                    FirstName = update.CallbackQuery.Message.From.FirstName,
                    LastName = update.CallbackQuery.Message.From.LastName,
                    Id = update.CallbackQuery.Message.Chat.Id,
                },
                UpdateType.Message => new AppUser
                {
                    UserName = update.Message.Chat.Username,
                    FirstName = update.Message.Chat.FirstName,
                    LastName = update.Message.Chat.LastName,
                    Id = update.Message.Chat.Id,
                }
            };
            var user = await dataContext.Users.FirstOrDefaultAsync(user => user.Id == newUser.Id);
            if (user != null) return user;

            var result = await dataContext.AddAsync(newUser);
            await dataContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<AppUser> SetUserPhone(long chatId,string _cachePhoneNumber)
        {
            var user = dataContext.Users.FirstOrDefault(c=>c.Id==chatId);
            if (user != null) user.PhoneNumber = _cachePhoneNumber;

            else await dataContext.AddAsync(user);
            await dataContext.SaveChangesAsync();
            return user;
        }
    }
}
