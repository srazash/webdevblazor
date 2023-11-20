using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorWebAssembly.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Data.Models.Interfaces;
using Components.RazorComponents;
using Blazored.SessionStorage;
using Components.Interfaces;
using BlazorWebAssembly.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<IBrowserStorage, BlogBrowserStorage>();

builder.Services.AddHttpClient("Public", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddHttpClient("Authenticated", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

builder.Services.AddTransient<IBlogApi, BlogApiWebClient>();

builder.Services.AddTransient<ILoginStatus, LoginStatusWasm>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";

    options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
}).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
