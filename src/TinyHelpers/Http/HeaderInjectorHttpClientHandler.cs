using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TinyHelpers.Http
{
    public class HeaderInjectorHttpClientHandler : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, Task<Dictionary<string, string>>> getHeaders;

        public HeaderInjectorHttpClientHandler(Func<HttpRequestMessage, Task<Dictionary<string, string>>> getHeaders, HttpMessageHandler? innerHandler = null)
            : base(innerHandler)
            => this.getHeaders = getHeaders ?? throw new ArgumentNullException(nameof(getHeaders));

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var headers = await getHeaders(request).ConfigureAwait(false);
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}