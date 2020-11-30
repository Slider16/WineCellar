using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WineCellar.Blazor.UI.Services;

namespace WineCellar.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<WineCellar.Blazor.UI.App>("app");

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient<IWineDataService, WineDataService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:7777");
            });

            builder.Services.AddHttpClient<IVendorDataService, VendorDataService>(client =>
            {
                client.BaseAddress = new Uri("http://localhost:7777");
            });

            //builder.Services.AddBlazorise(options =>
            //{
            //    options.ChangeTextOnKeyPress = true;
            //})
            //.AddBootstrapProviders()
            //.AddFontAwesomeIcons();

            // Disabled this default line to enable bits for Blazorise
            await builder.Build().RunAsync();

            //var host = builder.Build();
            //host.Services
            //    .UseBootstrapProviders()
            //    .UseFontAwesomeIcons();

            //await host.RunAsync();                
        }
    }
}
