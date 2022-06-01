using Microsoft.EntityFrameworkCore;
using TelegramBot.Commands;
using TelegramBot.Entities;
using TelegramBot.Models.DataContext;
using TelegramBot.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
string identityConntection = builder.Configuration.GetConnectionString("TelegramDbConnection");
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<UserDataContext>(opts => opts.UseSqlServer(identityConntection),ServiceLifetime.Singleton);
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<Bot>();
builder.Services.AddSingleton<ICommandExecutor, CommandExecutor>();
builder.Services.AddSingleton<PhoneConfirmationService>();
builder.Services.AddSingleton<BaseCommand, StartCommand>();
builder.Services.AddSingleton<BaseCommand, ConfirmCommand>();
builder.Services.AddSingleton<BaseCommand, AskCommand>();

builder.Services.AddMemoryCache();
var app = builder.Build();
app.Services.GetRequiredService<Bot>().GetBot().Wait();
app.MapControllers();

app.Run();

