using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Commands;
using TelegramBot.Models;

namespace TelegramBot.Services
{
    public class CommandExecutor:ICommandExecutor
    {
        private readonly List<BaseCommand> commands;
        private BaseCommand lastCommand;
        public CommandExecutor(IServiceProvider _provider)
        {
            commands=_provider.GetServices<BaseCommand>().ToList();
        }

        public async Task Execute(Update update)
        {
            if (update.Message.Chat == null && update.CallbackQuery == null) return;
            if (update.Message.Type == MessageType.Contact && update.Message.Contact != null)
            {
                await ExecuteCommand(CommandNames.AskCommand, update);
            }
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message?.Text)
                {
                    case "/start":
                        await ExecuteCommand(CommandNames.StartCommand, update);
                        return;
                }
            }

            switch (lastCommand?.Name)
            {
                case CommandNames.AskCommand:
                    switch (update.Message.Text)
                    {
                        case "Да":
                            await ExecuteCommand(CommandNames.ConfirmCommand,update);
                            return;
                        case "Нет":
                            await ExecuteCommand(CommandNames.StartCommand,update);
                            return;
                    }
                    break;
            }
        }

        private async Task ExecuteCommand(string commandName, Update update)
        {
            lastCommand = commands.First(x => x.Name == commandName);
            await lastCommand.ExecuteAsync(update);
        }
    }
}
