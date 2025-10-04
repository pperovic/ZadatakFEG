using HattrickApp.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace HattrickApp.Api.Persistence;

public class HattrickAppDbContext(DbContextOptions<HattrickAppDbContext> options) 
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }
    public DbSet<Offer> Offers { get; set; }
    public DbSet<OfferTip> OfferTips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserName)
                .HasMaxLength(50);
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasOne(w => w.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(w => w.Balance)
                .HasPrecision(18, 2);
        });

        modelBuilder.Entity<WalletTransaction>(entity =>
        {
            entity.HasOne(wt => wt.Wallet)
                .WithMany(w => w.WalletTransactions)
                .HasForeignKey(wt => wt.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(e => e.TransactionType)
                .HasConversion<string>()
                .HasMaxLength(20);
            
            entity.Property(w => w.Amount)
                .HasPrecision(18, 2);
        });
        
        modelBuilder.Entity<Offer>(entity =>
        {
            entity.Property(o => o.FirstCompetitor)
                .HasMaxLength(100); 

            entity.Property(o => o.SecondCompetitor)
                .HasMaxLength(100);

            entity.Property(e => e.SportType)
                .HasConversion<string>()
                .HasMaxLength(20);

            entity.HasMany(o => o.Tips)
                .WithOne(t => t.Offer)
                .HasForeignKey(t => t.OfferId);
        });
        
        modelBuilder.Entity<OfferTip>(entity =>
        {
            entity.Property(t => t.TipCode)
                .HasMaxLength(10);

            entity.Property(t => t.Quota)
                .HasPrecision(5, 2); 
        });
    }
}
