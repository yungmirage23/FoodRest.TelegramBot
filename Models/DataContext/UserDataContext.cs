using Microsoft.EntityFrameworkCore;
using TelegramBot.Entities;

namespace TelegramBot.Models.DataContext
{
    public class UserDataContext:DbContext
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
        {

        }
        public DbSet<AppUser> Users { get; set;}
    }
}
