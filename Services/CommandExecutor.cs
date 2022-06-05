using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Commands;
using TelegramBot.Entities;
using TelegramBot.Models;
using TelegramBot.Models.DataContext;

namespace TelegramBot.Services
{
    public class CommandExecutor:ICommandExecutor
    {
        private readonly List<BaseCommand> commands;
        private BaseCommand lastCommand;
        private UserDataContext dataContext;
        private IUserService userService;
        public CommandExecutor(IServiceProvider _provider, UserDataContext _dataContext, IUserService _userService)
        {
            dataContext=_dataContext;
            commands = _provider.GetServices<BaseCommand>().ToList();
            userService= _userService;
        }
        public async Task Execute(Update update)
        {
            if (update.Message.Chat == null && update.CallbackQuery == null) return;
            var user= await userService.GetOrCreate(update);
            if (update.Type == UpdateType.Message )
            {
                switch (update.Message?.Text)
                {
                    case "/start":
                        await ExecuteCommand(CommandNames.StartCommand, update, user);
                        return;
                }
            }
            if (update.Message.Type == MessageType.Contact && update.Message.Contact != null)
            {
                await ExecuteCommand(CommandNames.AskCommand, update, user);
                return;
            }

            string? userLocation = dataContext?.Users?.FirstOrDefault(u => u.Id == update.Message.Chat.Id)?.State;
            switch (userLocation)
            {
                case CommandNames.AskCommand:
                    switch (update.Message.Text)
                    {
                        case "Да":
                            await ExecuteCommand(CommandNames.ConfirmCommand,update, user);
                            return;
                        case "Нет":
                            await ExecuteCommand(CommandNames.StartCommand,update, user);
                            return;
                    }
                    break;
            }
        }
        private async Task ExecuteCommand(string commandName, Update update, AppUser user)
        {
            lastCommand = commands.First(x => x.Name == commandName);
            await lastCommand.ExecuteAsync(update, user);
            await userService.UserMove(user, commandName);
        }
    }
}
