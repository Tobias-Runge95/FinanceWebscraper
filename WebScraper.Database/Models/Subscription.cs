namespace WebScraper.Database.Models;

public class Subscription : BaseModel
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid TraderId { get; set; }
    public Trader Trader { get; set; }
}