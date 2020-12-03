using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using AdventOfCode._2020.Puzzles.Core;
using AdventOfCode._2020.Web.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AdventOfCode._2020.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<ISolutionHandler, SolutionHandler>();
            builder.Services.AddSingleton<IInputHandler, InputHandler>();
            builder.Services.AddSingleton<IVisualizerHandler, VisualizerHandler>();

            await builder.Build().RunAsync();
        }
    }
}
