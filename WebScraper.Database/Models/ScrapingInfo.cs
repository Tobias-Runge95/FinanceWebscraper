namespace WebScraper.Database.Models;

public class ScrapingInfo : BaseModel
{
    public DateOnly LastScrapedDate { get; set; }
}