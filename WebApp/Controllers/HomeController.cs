using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

public class HomeController : Controller
{    
    private readonly HttpClient _client;

    public HomeController(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _client.BaseAddress = new Uri("https://localhost:7185");
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public Task<IActionResult> Index()
    {
        StatesController statesController = new StatesController(_client);
        
        return statesController.Index();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
