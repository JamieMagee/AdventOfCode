using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace AdventOfCode._2020.Console
{
    public sealed class Configuration
    {
        private Configuration()
        {
        }

        public int Year { get; set; } = 2020;

        public string PuzzleProjectPath { get; set; } = Path.Combine("..", "..", "..", "..", "AdventOfCode.2020.Puzzles");

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
}