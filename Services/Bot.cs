using Telegram.Bot;
using TelegramBot.Models;
using System.Collections.Generic;
using TelegramBot.Commands;

namespace TelegramBot.Entities
{
    public class Bot
    {
        private TelegramBotClient botClient;
        private readonly IConfiguration configuration;

        public Bot(IConfiguration _configuration)
        {
            configuration=_configuration;
        }

        public async Task<TelegramBotClient> GetBot()
        {
            if (botClient != null)
                return botClient;
            botClient = new TelegramBotClient(configuration["Token"]);
            string hook = $"{configuration["Url"]}api/message/update";
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
