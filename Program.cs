using Microsoft.EntityFrameworkCore;
using TelegramBot.Commands;
using TelegramBot.Entities;
using TelegramBot.Models.DataContext;
using TelegramBot.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
string identityConntection = builder.Configuration.GetConnectionString("TelegramDbConnection");
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<UserDataContext>(opts => opts.UseSqlServer(identityConntection),ServiceLifetime.Scoped);
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<PhoneConfirmationService>();

builder.Services.AddSingleton<Bot>();
builder.Services.AddScoped<ICommandExecutor, CommandExecutor>();
builder.Services.AddScoped<BaseCommand, StartCommand>();
builder.Services.AddScoped<BaseCommand, ConfirmCommand>();
builder.Services.AddScoped<BaseCommand, AskCommand>();
builder.WebHost.UseUrls("http://localhost:8443");
builder.Services.AddMemoryCache();
var app = builder.Build();
app.Services.GetRequiredService<Bot>().GetBot().Wait();
app.MapControllers();

app.Run();

