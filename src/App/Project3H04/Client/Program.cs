using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Project3H04.Shared.Kunstwerken;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Project3H04.Client.Shared;
using Append.Blazor.Sidepanel;
using Blazored.Toast;
using Project3H04.Client.Services;

namespace Project3H04.Client {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            //AUTH
            builder.Services.AddOidcAuthentication(options => {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
                options.ProviderOptions.ResponseType = "code";
                //options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);  //=> hiervoor blazor 6 nodig !
            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>(); 
            
            //AUTH client
            builder.Services.AddHttpClient("ServerAPI",
            client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            //Toast
            builder.Services.AddBlazoredToast();

            //UnAUTH client voor paginas anonymous te zetten
            //builder.Services.AddHttpClient("BlazorApp.PublicServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient<PublicClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            
            //builder.Services.AddScoped<OrdersServ>();
            builder.Services.AddSingleton<CartState>();

            //sidepanel by THE Vertonghen
            builder.Services.AddSidepanel();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
              .CreateClient("ServerAPI"));

            //Storages injecteren:
            builder.Services.AddHttpClient<StorageService>();
            builder.Services.AddScoped<KunstwerkService>();
            builder.Services.AddScoped<KunstenaarService>();
            builder.Services.AddScoped<VeilingService>();

            await builder.Build().RunAsync();
        }
    }
}
