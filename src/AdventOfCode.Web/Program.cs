using System;
using System.Net.Http;
using System.Threading.Tasks;
using AdventOfCode.Core;
using AdventOfCode.Web.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AdventOfCode.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddSingleton(sp => new HttpClient
                {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
            builder.Services.AddSingleton<ISolutionHandler, SolutionHandler>();
            builder.Services.AddSingleton<IInputHandler, InputHandler>();
            builder.Services.AddSingleton<IVisualizerHandler, VisualizerHandler>();

            await builder.Build().RunAsync();
        }
    }
}