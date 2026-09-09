using System.Net;
using TinyHelpers.Http;

namespace TinyHelpers.Tests.Http;

public class HeaderInjectorHttpClientHandlerTests
{
    [Fact]
    public void Constructors_NullDelegates_Throw()
    {
        Assert.Throws<ArgumentNullException>(() => new HeaderInjectorHttpClientHandler(null!));
        Assert.Throws<ArgumentNullException>(() => new HeaderInjectorHttpClientHandler(null!, new StubHandler()));
        Assert.Throws<ArgumentNullException>(() => new QueryStringInjectorHttpClientHandler(null!));
        Assert.Throws<ArgumentNullException>(() => new QueryStringInjectorHttpClientHandler(null!, new StubHandler()));
        Assert.Throws<ArgumentNullException>(() => new AuthenticatedParameterizedHttpClientHandler(null!));
        Assert.Throws<ArgumentNullException>(() => new AuthenticatedParameterizedHttpClientHandler(null!, new StubHandler()));
    }

    [Fact]
    public async Task HeaderInjector_AddsHeadersAndForwardsRequest()
    {
        var inner = new StubHandler();
        using var client = new HttpClient(new HeaderInjectorHttpClientHandler(
            request => Task.FromResult(new Dictionary<string, string> { ["X-Test"] = request.Method.Method }), inner));

        var response = await client.GetAsync("https://example.test/path", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("GET", inner.Request!.Headers.GetValues("X-Test").Single());
    }

    [Fact]
    public async Task HeaderInjector_NullHeaders_ForwardsUnchangedRequest()
    {
        var inner = new StubHandler();
        using var client = new HttpClient(new HeaderInjectorHttpClientHandler(_ => Task.FromResult<Dictionary<string, string>>(null!), inner));

        await client.GetAsync("https://example.test/path", TestContext.Current.CancellationToken);

        Assert.Empty(inner.Request!.Headers);
    }
}
