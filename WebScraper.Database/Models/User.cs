using Microsoft.AspNetCore.Identity;

namespace WebScraper.Database.Models;

public class User : IdentityUser<Guid>
{
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public string ZIP { get; set; }
    public string Country { get; set; }
    public List<Subscription> Subscriptions { get; set; }
}