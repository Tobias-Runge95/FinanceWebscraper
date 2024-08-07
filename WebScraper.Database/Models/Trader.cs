namespace WebScraper.Database.Models;

public class Trader : BaseModel
{
    public string Name { get; set; }
    public string? Party { get; set; }
    public string ExtraInformation { get; set; }
    public IEnumerable<Trade> History { get; set; }
    public IEnumerable<Subscription> Subscriptions { get; set; }
    public string ActualBuyer { get; set; }

    public Trader(string name, string? party, string extraInformation, string actualBuyer)
    {
        Name = name;
        Party = party;
        ExtraInformation = extraInformation;
        ActualBuyer = actualBuyer;
        History = new List<Trade>();
    }
}