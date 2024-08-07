using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebScraper.Database.Models;

namespace WebScraper.Database;

public class WebScrapperDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public WebScrapperDbContext(DbContextOptions<WebScrapperDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Trader> Traders { get; set; }
    public DbSet<Trade> Trades { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<ScrapingInfo> ScrapingInfos { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("identity");
        
        // Subscription
        builder.Entity<Subscription>().HasOne(x => x.Trader).WithMany(x => x.Subscriptions).HasForeignKey(x => x.TraderId).IsRequired();
        builder.Entity<Subscription>().HasOne(x => x.User).WithMany(x => x.Subscriptions).HasForeignKey(x => x.UserId).IsRequired();
        
        // Trade
        builder.Entity<Trade>().HasOne(x => x.Trader).WithMany(x => x.History).HasForeignKey(x => x.TraderId).IsRequired();
        
        //ScrapingInfo
        builder.Entity<ScrapingInfo>().HasKey(x => x.Id);
    }
}