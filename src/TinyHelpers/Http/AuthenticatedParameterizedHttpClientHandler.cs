using System.Net.Http.Headers;

namespace TinyHelpers.Http;

public class AuthenticatedParameterizedHttpClientHandler : DelegatingHandler
{
    private readonly Func<HttpRequestMessage, Task<string>> getToken;
    private readonly bool checkAuthorizationHeader;

    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, bool checkAuthorizationHeader = true)
    {
        this.getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        this.checkAuthorizationHeader = checkAuthorizationHeader;
    }

    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, HttpMessageHandler innerHandler)
        : this(getToken, true, innerHandler)
    {
    }

    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, bool checkAuthorizationHeader, HttpMessageHandler innerHandler)
    : base(innerHandler)
    {
        this.getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        this.checkAuthorizationHeader = checkAuthorizationHeader;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // See if the request has an authorize header
        var auth = request.Headers.Authorization;
        if (!checkAuthorizationHeader || request.Headers.Authorization is not null)
        {
            var token = await getToken(request).ConfigureAwait(false);
            request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, token);
        }

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
