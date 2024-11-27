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
    // _client instance
    public static HttpClient _client = new HttpClient();

    public static StatesController StatesController = new StatesController(_client);

    private static async Task Main(string[] args)
    {
        // run _client async
        await RunAsync();

        var builder = WebApplication.CreateBuilder(args);        

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

    async static Task RunAsync() 
    {
        // set the base address of the _client
        _client.BaseAddress = new Uri("https://localhost:7185");
        // remove headers
        _client.DefaultRequestHeaders.Accept.Clear();
        // add header json
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        await StatesController.GetAllStates();
        await StatesController.GetStateById("Tennessee");
        //await CreateState();
        //await UpdateState("Lucianna");
        //await DeleteState("Lucianna");
    }

    

}