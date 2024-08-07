using Microsoft.EntityFrameworkCore;
using WebScraper.Database;
using WebScraper.Database.Models;

namespace WebScrapper.Core;

public interface IScrapingInfoRepository
{
    Task<ScrapingInfo> GetScrapingInfo();
    Task UpdateScrapingInfo(ScrapingInfo scrapingInfo, DateOnly date);
    Task CreateScrapingInfo(ScrapingInfo scrapingInfo);
}

public class ScrapingInfoRepository : IScrapingInfoRepository
{
    private readonly WebScrapperDbContext _webScrapperDbContext;

    public ScrapingInfoRepository(WebScrapperDbContext webScrapperDbContext)
    {
        _webScrapperDbContext = webScrapperDbContext;
    }

    public async Task<ScrapingInfo> GetScrapingInfo()
    {
        return await _webScrapperDbContext.ScrapingInfos.FirstAsync();
    }

    public async Task UpdateScrapingInfo(ScrapingInfo scrapingInfo, DateOnly date)
    {
        _webScrapperDbContext.ScrapingInfos.Update(scrapingInfo);
        scrapingInfo.LastScrapedDate = date;
        scrapingInfo.Updated = DateOnly.Parse(DateTime.Now.Date.ToString("d"));
        await _webScrapperDbContext.SaveChangesAsync();
    }

    public async Task CreateScrapingInfo(ScrapingInfo scrapingInfo)
    {
        await _webScrapperDbContext.ScrapingInfos.AddAsync(scrapingInfo);
        await _webScrapperDbContext.SaveChangesAsync();
    }
}