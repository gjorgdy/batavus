using Core.Services;
using CoreModules.Stats.MarvelRivals;
using dotenv.net;
using Microsoft.Extensions.Caching.Memory;

namespace TestProject1;

public class MarvelRivalsApiTests
{
    [SetUp]
    public void Setup()
    {
        DotEnv.Load();
    }

    private static readonly string[] Actual = ["Yordeaux"];

    [Test]
    public async Task TestPlayerResponse()
    {
        var client = new HttpClient();
        try
        {
            var res = await new MarvelRivalsPlayerService(new MemoryCache(new MemoryCacheOptions()), client, new LoggerService()).GetUser("454597044");
            await Console.Out.WriteLineAsync("Last update : " + res.Updates.Last);
            await Console.Out.WriteLineAsync("Player : " + res.Data.Name);
            await Console.Out.WriteLineAsync("Icon : " + res.Data.Icon.Url);
            await Console.Out.WriteLineAsync("Players team : " + res.Data.Team);
            Assert.That(Actual, Does.Contain(res.Data.Name));
        }
        catch (Exception e)
        {
            Console.Out.WriteLine(e);
            Assert.Fail();
        }
    }
}