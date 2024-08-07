namespace WebScraper.Database.Models;

public class Trade : BaseModel
{
    public Guid TraderId { get; set; }
    public string Issuer { get; set; }
    public string Published { get; set; }
    public string Traded { get; set; }
    public string Filed { get; set; }
    public string Owner { get; set; }
    public string Type { get; set; }
    public string Size { get; set; }
    public string Price { get; set; }
    public string TradeId { get; set; }
    public Trader Trader { get; set; }
}