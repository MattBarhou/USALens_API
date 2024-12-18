using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.Controllers;
using Microsoft.AspNetCore.JsonPatch;

internal class Program
{
    // client instance
    public static HttpClient _client = new HttpClient();
    // initialize controllers
    public static HomeController HomeController = new HomeController(_client);


    private static async Task Main(string[] args)
    {
        // run _client async
        await RunAsync();

        var builder = WebApplication.CreateBuilder(args);        

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Register HttpClient for dependency injection
        builder.Services.AddHttpClient();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}");

        app.Run();
    }

    async static Task RunAsync() 
    {
        // set the base address of the _client
        _client.BaseAddress = new Uri("https://localhost:8081");
        // remove headers
        _client.DefaultRequestHeaders.Accept.Clear();
        // add header json
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));        
    }
}