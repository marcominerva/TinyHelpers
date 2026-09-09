using System.Net;
using System.Net.Http.Headers;
using TinyHelpers.Http;

namespace TinyHelpers.Tests.Http;

public class AuthenticatedParameterizedHttpClientHandlerTests
{
    [Fact]
    public async Task AuthenticatedHandler_WithExistingHeader_ReplacesTokenAndPreservesScheme()
    {
        var inner = new StubHandler();
        using var client = new HttpClient(new AuthenticatedParameterizedHttpClientHandler(_ => Task.FromResult("new-token"), inner));
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://example.test");
        request.Headers.Authorization = new AuthenticationHeaderValue("Custom", "old-token");

        await client.SendAsync(request, TestContext.Current.CancellationToken);

        Assert.Equal("Custom", inner.Request!.Headers.Authorization!.Scheme);
        Assert.Equal("new-token", inner.Request.Headers.Authorization.Parameter);
    }

    [Fact]
    public async Task AuthenticatedHandler_DefaultBehaviorWithoutHeader_DoesNotRequestToken()
    {
        var calls = 0;
        var inner = new StubHandler();
        using var client = new HttpClient(new AuthenticatedParameterizedHttpClientHandler(_ =>
        {
            calls++;
            return Task.FromResult("token");
        }, inner));

        await client.GetAsync("https://example.test", TestContext.Current.CancellationToken);

        Assert.Equal(0, calls);
        Assert.Null(inner.Request!.Headers.Authorization);
    }

    [Fact]
    public async Task AuthenticatedHandler_CheckDisabled_AddsConfiguredScheme()
    {
        var inner = new StubHandler();
        using var handler = new AuthenticatedParameterizedHttpClientHandler(
            _ => Task.FromResult("token"), null, false, inner, "ApiKey");
        using var client = new HttpClient(handler);

        await client.GetAsync("https://example.test", TestContext.Current.CancellationToken);

        Assert.Equal(new AuthenticationHeaderValue("ApiKey", "token"), inner.Request!.Headers.Authorization);
    }

    [Fact]
    public async Task AuthenticatedHandler_Unauthorized_RefreshesAndRetriesOnce()
    {
        var tokens = new Queue<string>(["old", "new"]);
        var refreshed = 0;
        var inner = new StubHandler(HttpStatusCode.Unauthorized, HttpStatusCode.OK);
        using var client = new HttpClient(new AuthenticatedParameterizedHttpClientHandler(
            _ => Task.FromResult(tokens.Dequeue()),
            _ =>
            {
                refreshed++;
                return Task.CompletedTask;
            }, false, inner));

        var response = await client.GetAsync("https://example.test", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, inner.CallCount);
        Assert.Equal(1, refreshed);
        Assert.Equal("new", inner.Request!.Headers.Authorization!.Parameter);
    }

    [Fact]
    public async Task AuthenticatedHandler_RefreshThrows_ReturnsOriginalUnauthorizedResponse()
    {
        var inner = new StubHandler(HttpStatusCode.Unauthorized);
        using var client = new HttpClient(new AuthenticatedParameterizedHttpClientHandler(
            _ => Task.FromResult("token"),
            _ => throw new InvalidOperationException(), false, inner));

        var response = await client.GetAsync("https://example.test", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Equal(1, inner.CallCount);
    }
}
