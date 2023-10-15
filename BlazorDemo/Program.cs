using BlazorDemo.Data;
using BlazorDemo.HealthCheck;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//configure health checks
builder.Services
    .AddHealthChecks()
    .AddCheck("testing", () => HealthCheckResult.Healthy("test health")) //testing custom check
    .AddCheck<MemoryHealthCheck>("Memory");

builder.Services.AddHealthChecksUI(opt =>
{
    opt.AddHealthCheckEndpoint("Health Check Demo API", "/_health");
    opt.SetEvaluationTimeInSeconds(15);
}).AddInMemoryStorage();

builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/_health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(opt => opt.UIPath = "/dashboard");


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");



app.Run();
