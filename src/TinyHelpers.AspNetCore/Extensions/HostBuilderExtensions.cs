using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace TinyHelpers.AspNetCore.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddJsonConfigurationFile(this ConfigureHostBuilder hostBuilder, string fileName, bool optional = true, bool reloadOnChange = true)
        => hostBuilder.ConfigureAppConfiguration((_, configuration) =>
            {
                configuration.AddJsonFile(fileName, optional, reloadOnChange);
            });
}
