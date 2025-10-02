using HattrickApp.Api.Entities;
using HattrickApp.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HattrickApp.Api.Seeder;

public static class DbSeeder
{
    private const string TestUsernaName = "testuser";
    public static async Task SeedAsync(HattrickAppDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        await SeedUsersAsync(dbContext);
        await SeedWalletsAsync(dbContext);
        
        await dbContext.SaveChangesAsync();
    }
    
    private static async Task SeedUsersAsync(HattrickAppDbContext dbContext)
    {
        if (dbContext.Users.Any())
        {
            return;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = TestUsernaName
        };

       await dbContext.Users.AddAsync(user);
    }

    private static async Task SeedWalletsAsync(HattrickAppDbContext dbContext)
    {
        if (await dbContext.Wallets.AnyAsync())
        {
            return;
        }

        User? user =  dbContext.Users.Local.FirstOrDefault(u => u.UserName == TestUsernaName);
        if (user is null)
        {
            throw new InvalidOperationException("User must exist before creating wallet.");
        }

        var wallet = new Wallet
        {
            Id = Guid.NewGuid(),
            User = user,
            Balance = 0m
        };

        await dbContext.Wallets.AddAsync(wallet);
    }
}
