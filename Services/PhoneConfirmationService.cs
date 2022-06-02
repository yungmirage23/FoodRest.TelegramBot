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
            int codeCache;
            if(cache.TryGetValue(_phoneNumber, out codeCache))
                return codeCache;
            var rnd = new Random();
            int code = rnd.Next(1000, 9999);
            cache.Set(_phoneNumber, code, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            });
            return code;
        }

        public int GetConfirmationCode(string _phoneNumber)
        {
            int code;
            if (cache.TryGetValue(_phoneNumber, out code))
            {
                return code;
            }
            return 0;
        }
        public void SavePhoneInCache(long chatId,string phoneNumberCache)
        {
            var cacheentryoptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));
            cache.Set(chatId, phoneNumberCache.Replace("+",""), cacheentryoptions);
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
