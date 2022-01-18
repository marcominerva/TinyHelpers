using System.Net;
using System.Net.Http.Headers;

namespace TinyHelpers.Http;

public class AuthenticatedParameterizedHttpClientHandler : DelegatingHandler
{
    public const string BearerAuthorizationScheme = "Bearer";

    private readonly Func<HttpRequestMessage, Task<string>> getToken;
    private readonly Func<HttpRequestMessage, Task>? refreshToken;
    private readonly bool checkAuthorizationHeader;
    private readonly string authorizationScheme;

    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, Func<HttpRequestMessage, Task>? refreshToken = null, bool checkAuthorizationHeader = true, string authorizationScheme = BearerAuthorizationScheme)
    {
        this.getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        this.refreshToken = refreshToken;
        this.checkAuthorizationHeader = checkAuthorizationHeader;
        this.authorizationScheme = authorizationScheme;
    }

    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, HttpMessageHandler innerHandler, string authorizationScheme = BearerAuthorizationScheme)
        : this(getToken, null, true, innerHandler, authorizationScheme)
    {
    }

    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, Func<HttpRequestMessage, Task>? refreshToken, HttpMessageHandler innerHandler, string authorizationScheme = BearerAuthorizationScheme)
    : this(getToken, refreshToken, true, innerHandler, authorizationScheme)
    {
    }

    public AuthenticatedParameterizedHttpClientHandler(Func<HttpRequestMessage, Task<string>> getToken, Func<HttpRequestMessage, Task>? refreshToken, bool checkAuthorizationHeader, HttpMessageHandler innerHandler, string authorizationScheme = BearerAuthorizationScheme)
    : base(innerHandler)
    {
        this.getToken = getToken ?? throw new ArgumentNullException(nameof(getToken));
        this.refreshToken = refreshToken;
        this.checkAuthorizationHeader = checkAuthorizationHeader;
        this.authorizationScheme = authorizationScheme;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await CheckAddTokenAsync(request).ConfigureAwait(false);

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.Unauthorized && refreshToken != null)
        {
            try
            {
                await refreshToken(request).ConfigureAwait(false);
                await CheckAddTokenAsync(request).ConfigureAwait(false);

                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
            }
        }

        return response;

        async Task CheckAddTokenAsync(HttpRequestMessage request)
        {
            // See if the request has an authorize header
            var auth = request.Headers.Authorization;
            if (request.Headers.Authorization is not null || !checkAuthorizationHeader)
            {
                var token = await getToken(request).ConfigureAwait(false);
                request.Headers.Authorization = new AuthenticationHeaderValue(auth?.Scheme ?? authorizationScheme, token);
            }
        }
    }
}
