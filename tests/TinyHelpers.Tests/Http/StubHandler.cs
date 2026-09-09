using System.Net;

namespace TinyHelpers.Tests.Http;

internal sealed class StubHandler(params HttpStatusCode[] statusCodes) : HttpMessageHandler
{
    public int CallCount { get; private set; }
    public HttpRequestMessage? Request { get; private set; }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Request = request;
        var index = Math.Min(CallCount, statusCodes.Length - 1);
        CallCount++;
        var statusCode = statusCodes.Length == 0 ? HttpStatusCode.OK : statusCodes[index];
        return Task.FromResult(new HttpResponseMessage(statusCode));
    }
}
