using Cardmanagement.WebUI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.Authorization; //sec
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(); //sec
builder.Services.AddServerSideBlazor();

builder.Services.AddOptions(); //sec
builder.Services.AddAuthorizationCore(); //sec
builder.Services.AddScoped<CustomStateProvider>(); //sec
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomStateProvider>()); //sec

builder.Services.AddBlazoredLocalStorage();

//httpClient for the cards management API
builder.Services.AddHttpClient<CardAPIService>(client =>
{
    client.BaseAddress = new Uri(configuration["CardAPI:baseUri"]);
    client.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control", "no-cache");
    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new SocketsHttpHandler()
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(15)
    };
})
.SetHandlerLifetime(Timeout.InfiniteTimeSpan);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); //sec
app.UseAuthorization(); //sec
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
