using Telegram.Bot;
using TelegramBot.Models;
using System.Collections.Generic;

namespace TelegramBot.Entities
{
    public class Bot
    {
        private static TelegramBotClient botClient;
        private static List<Command> commandsList;
        public static IReadOnlyList<Command> Commands => commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (botClient == null)
                return botClient;

            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());
            botClient = new TelegramBotClient(AppSettings.Key);
            string hook = string.Format(AppSettings.Url, "api/message/updage");
            await botClient.SetWebhookAsync(hook);
            return botClient;
        }
    }
}
