using Microsoft.Extensions.Hosting;
using PuppeteerSharp;
using WebScrapper.Core.Services;

namespace WebScrapper.Core;

public class CapitolTradesScraper
{
    private readonly Writer _writer;
    private IBrowser _browser;
    private Dictionary<string, string> Row;
    private List<Dictionary<string, string>> Table = new();
    private readonly List<string> RowHeaders = new() { "Politician", "Issuer", "PubDate", "Traded", "ReportingGab", "Owner", "Type", "Value", "Price", "Id" };
    
    public CapitolTradesScraper(Writer writer)
    {
        _writer = writer;
    }

    public async Task StartScraping()
    {
        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
        _browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });

        var page = await _browser.NewPageAsync();
        await page.GoToAsync("https://www.capitoltrades.com/trades?sortBy=-pubDate&pageSize=96");
        try
        {
            await Scrape(page);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            await page.CloseAsync();
            await _browser.CloseAsync();
        }
        await _writer.WriteScrapedInfo(Table);
    }

    private async Task Scrape(IPage page)
    {
        try
        {
            await page.WaitForSelectorAsync("body > div > main > main > article > section > div.q-table-wrapper > div.trade-table-scroll-wrapper > table", new WaitForSelectorOptions() { Timeout = 1000 });
            var table = await page.QuerySelectorAsync(
                "body > div > main > main > article > section > div.q-table-wrapper > div.trade-table-scroll-wrapper > table");
            var tableBody = await table.QuerySelectorAsync(
                "body > div > main > main > article > section > div.q-table-wrapper > div.trade-table-scroll-wrapper > table > tbody");
            var tableRows = await tableBody.QuerySelectorAllAsync(
                "body > div > main > main > article > section > div.q-table-wrapper > div.trade-table-scroll-wrapper > table > tbody > tr");
            await RunThroughTableRows(tableRows);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task RunThroughTableRows(IElementHandle[] tableRows)
    {
        for (int i = 0; i < tableRows.Length; i++)
        {
            var tableRowData = await tableRows[i].QuerySelectorAllAsync("td");
            await RunThroughTableRowData(tableRowData);
            Table.Add(Row);
        }
    }

    private async Task RunThroughTableRowData(IElementHandle[] tableRowData)
    {
        string text = String.Empty;
        Row = new();
        for (int i = 0; i < tableRowData.Length; i++)
        {
            if (i == 0)
            {
                text = await RetrieveTraderInfo(tableRowData[i]);
            }
            else
            {
                var innerText = await tableRowData[i].GetPropertyAsync("innerText");
                text = await innerText.JsonValueAsync<string>();
            }
            
            Row.Add(RowHeaders[i], text);
        }
    }

    private async Task<string> RetrieveTraderInfo(IElementHandle handle)
    {
        string information = String.Empty;
        List<IElementHandle> handles = new List<IElementHandle>();
        handles.Add( await handle.QuerySelectorAsync("div > div > h2"));
        handles.Add(await handle.QuerySelectorAsync("div > div > span.party"));
        handles.Add(await handle.QuerySelectorAsync("div > div > span.chamber"));
        handles.Add(await handle.QuerySelectorAsync("div > div > span.us-state-compact"));

        for (int i = 0; i < 4; i++)
        {
            var innerText = await handles[i].GetPropertyAsync("innerText");
            information += $"{await innerText.JsonValueAsync<string>()};";
        }

        return information;
    }
}