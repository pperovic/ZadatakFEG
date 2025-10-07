using HattrickApp.Api.Common;
using HattrickApp.Api.Entities;
using HattrickApp.Api.Enums;
using HattrickApp.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HattrickApp.Api.Seeder;

public static class DbSeeder
{
    private const string TestUserName = "testuser";
    
    public static async Task SeedAsync(HattrickAppDbContext dbContext)
    {
        await dbContext.Database.MigrateAsync();

        await SeedUsersAsync(dbContext);
        await SeedWalletsAsync(dbContext);
        await SeedOffersAsync(dbContext);
        
        await dbContext.SaveChangesAsync();
    }
    
    private static async Task SeedUsersAsync(HattrickAppDbContext dbContext)
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = TestUserName
        };

       await dbContext.Users.AddAsync(user);
    }

    private static async Task SeedWalletsAsync(HattrickAppDbContext dbContext)
    {
        if (await dbContext.Wallets.AnyAsync())
        {
            return;
        }

        User? user =  dbContext.Users.Local.FirstOrDefault(u => u.UserName == TestUserName);
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
    
    private static async Task SeedOffersAsync(HattrickAppDbContext dbContext)
    {
        if (await dbContext.Offers.AnyAsync())
        {
            return;
        }

        var footballOffer = new Offer
        {
            Id = Guid.NewGuid(),
            SportType = SportType.Football,
            FirstCompetitor = "Manchester United",
            SecondCompetitor = "Chelsea",
            StartTime = DateTimeOffset.Now.AddDays(1),
            IsTopOffer = true,
            Tips = new List<OfferTip>
            {
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.FirstCompetitorWin, Quota = 1.85m },
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.Draw, Quota = 3.40m },
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.SecondCompetitorWin, Quota = 2.95m },
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.FirstCompetitorWinOrDraw, Quota = 1.95m },
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.SecondCompetitorWinOrDraw, Quota = 2.95m },
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.FirsOrSecondCompetitorWin, Quota = 1.35m }
            }
        };
        
        
        var footBallOfferNoQuotas = new Offer
        {
            Id = Guid.NewGuid(),
            SportType = SportType.Football,
            FirstCompetitor = "Real Madrid",
            SecondCompetitor = "Barcelona",
            StartTime = DateTimeOffset.Now.AddDays(1),
            IsTopOffer = true,
            Tips = new List<OfferTip>
            {
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.FirstCompetitorWin },
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.Draw },
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.SecondCompetitorWin }
            }
        };
        

        var tennisOffer = new Offer
        {
            Id = Guid.NewGuid(),
            SportType = SportType.Tennis,
            FirstCompetitor = "Rafael Nadal",
            SecondCompetitor = "Roger Federer",
            StartTime = DateTimeOffset.Now.AddDays(1),
            IsTopOffer = false,
            Tips = new List<OfferTip>
            {
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.FirstCompetitorWin, Quota = 1.65m },
                new() { Id = Guid.NewGuid(), TipCode = TipRegistry.SecondCompetitorWin, Quota = 2.20m }
            }
        };

        await dbContext.Offers.AddRangeAsync(footballOffer, footBallOfferNoQuotas, tennisOffer);
    }
}
