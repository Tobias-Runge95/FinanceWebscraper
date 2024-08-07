namespace WebScraper.Database.Models;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateOnly Created { get; set; }
    public DateOnly? Updated { get; set; }
}