using TinyHelpers.Http;

namespace TinyHelpers.Tests.Http;

public class QueryStringInjectorHttpClientHandlerTests
{
    [Fact]
    public async Task QueryStringInjector_MergesAndEncodesParameters()
    {
        var inner = new StubHandler();
        using var client = new HttpClient(new QueryStringInjectorHttpClientHandler(
            _ => Task.FromResult(new Dictionary<string, string> { ["added"] = "a b" }), inner));

        await client.GetAsync("https://example.test/path?existing=1", TestContext.Current.CancellationToken);

        Assert.Equal("?existing=1&added=a+b", inner.Request!.RequestUri!.Query);
    }

    [Fact]
    public async Task QueryStringInjector_NullParameters_PreservesExistingQuery()
    {
        var inner = new StubHandler();
        using var client = new HttpClient(new QueryStringInjectorHttpClientHandler(_ => Task.FromResult<Dictionary<string, string>>(null!), inner));

        await client.GetAsync("https://example.test/path?existing=1", TestContext.Current.CancellationToken);

        Assert.Equal("?existing=1", inner.Request!.RequestUri!.Query);
    }
}
