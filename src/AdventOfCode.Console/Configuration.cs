using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode.Console;

internal sealed class Configuration
{
    private Configuration()
    {
    }

    public required string SessionCookie { get; set; }

    public static Configuration Load()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true, true)
            .Build();

        var configuration = new Configuration
        {
            SessionCookie = string.Empty
        };
        config.Bind(configuration);

        return configuration;
    }
}
