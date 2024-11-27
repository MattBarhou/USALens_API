using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using WebApp.Models;
using Microsoft.AspNetCore.JsonPatch;

internal class Program
{
    // client instance
    static HttpClient client = new HttpClient();

    private static async Task Main(string[] args)
    {
        // run client async
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

    static async Task RunAsync() 
    {
        // set the base address of the client
        client.BaseAddress = new Uri("https://localhost:7185");
        // remove headers
        client.DefaultRequestHeaders.Accept.Clear();
        // add header json
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //await GetAllStates();
        //await GetStateById("Tennessee");
        //await CreateState();
        //await UpdateState("Lucianna");
        //await DeleteState("Lucianna");
    }

    // GET ALL STATES
    static async Task GetAllStates() 
    {
        try
        {
            string json;
            HttpResponseMessage response;
            response = await client.GetAsync("/api/states");
            Debug.WriteLine("checking response and get all states");
            if (response.IsSuccessStatusCode)
            {
                json = await response.Content.ReadAsStringAsync();
                IEnumerable<State> states = JsonConvert.DeserializeObject<IEnumerable<State>>(json);

                foreach (State state in states)
                {
                    // print each state
                    Debug.WriteLine(state.StateName);
                }
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    // GET STATE BY ID
    static async Task GetStateById(string stateName)
    {
        try
        {
            State state;
            HttpResponseMessage response;
            response = await client.GetAsync($"/api/states/{stateName}");
            if (response.IsSuccessStatusCode)
            {
                state = await response.Content.ReadAsAsync<State>();

                // print the state details 
                Debug.WriteLine($"\nSTATE NAME: {stateName} \n" +
                    $"STATE DETAILS:\n"
                    + state.Abbreviation + "\n"
                    + state.Capital + "\n"
                    + state.Population + "\n"
                    + state.Region + "\n");
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    // POST - CREATE STATE
    static async Task CreateState()
    {
        string json;
        try
        {
            State state = new State
            {
                StateName = "Lucianna",
                Abbreviation = "LN",
                Capital = "lala",
                Population = 689757,
                Area = 41235,
                Region = "South",
                TimeZones = new List<string> { "Central" },
                FlagUrl = "https://matt-barhou.s3.amazonaws.com/state-flags/TN.png"
            };

            HttpResponseMessage response;
            response = await client.PostAsJsonAsync("/api/states", state);
            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("State created successfully: " + response.StatusCode);
                response.EnsureSuccessStatusCode();
                Debug.WriteLine("Added resourse as: " + response.Headers.Location);

                json = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    // PUT - UPDATE STATE
    static async Task UpdateState(string stateName) 
    {
        string json;
        try
        {
            // update content
            State state = new State
            {
                StateName = "Lucianna",
                Abbreviation = "FF",
                Capital = "lala",
                Population = 689757,
                Area = 41235,
                Region = "South",
                TimeZones = new List<string> { "Central" },
                FlagUrl = "https://matt-barhou.s3.amazonaws.com/state-flags/TN.png"
            };

            HttpResponseMessage response;
            json = JsonConvert.SerializeObject(state);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            //update item
            response = await client.PutAsync($"/api/states/{stateName}", content);
            Debug.WriteLine("STATE UPDATE STATUS: " + response.StatusCode);
            response.EnsureSuccessStatusCode();

            await GetStateById("Lucianna");

        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

    // PATCH - PARTIALLY UPDATE STATE 
    static async Task PatchState(string stateName, JsonPatchDocument<State> patchDocument)
    {
        try
        {
            // Serialize the JsonPatchDocument to JSON
            string json = JsonConvert.SerializeObject(patchDocument);
            StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            // Send the PATCH request
            HttpResponseMessage response = await client.PatchAsync($"/api/states/{stateName}", content);

            // Check the response
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                State updatedState = JsonConvert.DeserializeObject<State>(responseBody);

                // Print updated state details
                Debug.WriteLine("Updated State:");
                Debug.WriteLine($"Name: {updatedState.StateName}");
                Debug.WriteLine($"Capital: {updatedState.Capital}");
                Debug.WriteLine($"Population: {updatedState.Population}");
            }
            else
            {
                Debug.WriteLine($"Failed to patch state. Status Code: {response.StatusCode}");
                string errorDetails = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error Details: {errorDetails}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
        }
    }


    // DELETE STATE
    static async Task DeleteState(string stateName) 
    {
        try 
        {
            HttpResponseMessage response;
            response = await client.DeleteAsync($"/api/states/{stateName}");
            Debug.WriteLine($"STATUS FROM DELETE: {response.StatusCode}");
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }
    }

}