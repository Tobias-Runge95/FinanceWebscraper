using WebScraper.Database.Models;

namespace WebScrapper.Core.Services;

public class Writer
{
    private readonly ITradeRepository _tradeRepository;
    private readonly ITraderRepository _traderRepository;
    private readonly IScrapingInfoRepository _scrapingInfoRepository;

    public Writer(ITradeRepository tradeRepository, ITraderRepository traderRepository, IScrapingInfoRepository scrapingInfoRepository)
    {
        _tradeRepository = tradeRepository;
        _traderRepository = traderRepository;
        _scrapingInfoRepository = scrapingInfoRepository;
    }

    public async Task WriteScrapedInfo(List<Dictionary<string, string>> info)
    {
        var trader = MapInfoToTrader(info);
        var dbTrader = await _traderRepository.GetAllTraders();
        var newTrader = FindNewTrader(trader, dbTrader);
        if (newTrader.Count > 0)
        {
            await AddNewTrader(newTrader);
        }
        
        dbTrader.AddRange(newTrader);
        var scrapingInfo = await _scrapingInfoRepository.GetScrapingInfo();
        var newTrades = MapInfoToTrades(info, dbTrader, scrapingInfo.LastScrapedDate);
        await _tradeRepository.CreateMultipleTrades(newTrades);
        await _scrapingInfoRepository.UpdateScrapingInfo(scrapingInfo, DateOnly.Parse(DateTime.Now.ToString("d")));
    }

    private List<Trader> MapInfoToTrader(List<Dictionary<string, string>> info)
    {
        return info.Select(x =>
        {
            var politician = SplitPoliticianString(x["Politician"]);
            var trader = new Trader(politician[0], politician[1], politician[2], x["Owner"]);
            trader.Created = DateOnly.Parse(DateTime.Now.Date.ToString("d"));
            return trader;
        }).ToList();
    }

    private List<Trader> FindNewTrader(List<Trader> newList, List<Trader> dbTrader)
    {
        List<int> positions = new();
        List<Trader> result = new();
        for (int i = 0; i < newList.Count; i++)
        {
            if (dbTrader.All(x => x.Name != newList[i].Name))
            {
                positions.Add(i);
            }
        }

        foreach (int position in positions)
        {
            newList[position].Id = Guid.NewGuid();
            if (result.FindIndex(x => x.Name == newList[position].Name) >= 0)
            {
                continue;
            }
            result.Add(newList[position]);
        }

        return result;
    }

    private async Task AddNewTrader(List<Trader> newTrader)
    {
        await _traderRepository.CreateTraders(newTrader);
    }

    private string[] SplitPoliticianString(string politician)
        => politician.Split(";");

    private List<Trade> MapInfoToTrades(List<Dictionary<string, string>> infos, List<Trader> traders, DateOnly date)
    {
        List<Trade> newTrades = new();
        foreach (Dictionary<string,string> info in infos)
        {
            DateOnly testDate;
            var testBool = DateOnly.TryParse(info["PubDate"], out testDate);
            if (testBool && testDate.CompareTo(date) < 0)
            {
                continue;
            }
            var politician = SplitPoliticianString(info["Politician"]);
            var trader = traders.Find(x => x.Name == politician[0]);
            newTrades.Add(new Trade()
            {
                Created = DateOnly.Parse(DateTime.Now.Date.ToString("d")),
                Filed = info["ReportingGab"],
                Id = Guid.NewGuid(),
                Issuer = info["Issuer"],
                Type = info["Type"],
                Owner = info["Owner"],
                Price = info["Price"],
                Published = info["PubDate"],
                Size = info["Value"],
                Traded = info["Traded"],
                TradeId = info["Id"],
                TraderId = trader.Id
            });
        }

        return newTrades;
    }
}