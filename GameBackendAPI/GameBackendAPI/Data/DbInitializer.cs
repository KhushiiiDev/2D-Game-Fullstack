using GameBackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GameBackendAPI.Data
{
    public static class DbInitializer
    {
        public static void Seed(GameDbContext context)
        {
            context.Database.Migrate();
            if (!context.Users.Any())
            {
                context.Users.Add(new User { Username = "admin", Email = "admin@game.com", PasswordHash = "adminhash", Role = "Admin" });
                context.SaveChanges();
            }
        }
    }
}
