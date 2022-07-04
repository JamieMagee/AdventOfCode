using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode.Console;

public sealed class Configuration
{
    private Configuration()
    {
    }

    public string SessionCookie { get; set; }

    public static Configuration Load()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true, true)
            .Build();

        var configuration = new Configuration();
        config.Bind(configuration);

        return configuration;
    }
}
