using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OrchidManagement.Client;
using OrchidManagement.Client.Auth;
using OrchidManagement.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri("https://localhost:7066/") // Your API URL
});

// Add Auth Services
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthService>();

// Add API Services
builder.Services.AddScoped<AccountService>();

await builder.Build().RunAsync();