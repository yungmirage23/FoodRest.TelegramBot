using Microsoft.Extensions.Caching.Memory;
using Telegram.Bot.Types;

namespace TelegramBot.Services
{
    public class PhoneConfirmationService
    {
        private IMemoryCache cache;
        private IUserService userService;
        public PhoneConfirmationService(IMemoryCache _cache, IUserService _userService)
        {
            cache = _cache;
            userService = _userService;
        }

        public int CreateConfirmationCode(string _phoneNumber)
        {
            var rnd=new Random();
            int code = rnd.Next(1000, 9999);
            cache.Set(_phoneNumber, code, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return code ;
        }
        public void SavePhoneInCache(long chatId,string phoneNumberCache)
        {
            cache.Set(chatId, phoneNumberCache, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

        public async Task<bool> SetPhoneFromCache(Update update)
        {
            long chatId= update.Message.Chat.Id;
            string cachePhoneNumber;
            if (cache.TryGetValue(chatId,out cachePhoneNumber))
            {
                await userService.SetUserPhone(chatId,cachePhoneNumber);
                cache.Remove(chatId);
                return true;
            }
            return false;
        }
    }
}
