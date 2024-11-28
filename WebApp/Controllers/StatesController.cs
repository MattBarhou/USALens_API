using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Controllers
{
    public class StatesController :  Controller
    {
        private readonly HttpClient _client;

        public StatesController(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.BaseAddress = new Uri("https://localhost:7185");
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<IActionResult> Index()
        {
            IEnumerable<State> states = new List<State>();
            try
            {
                states = await GetAllStates();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return View(states);
        }

        [Route("Home/Details/{stateName}")]
        public async Task<IActionResult> Details(string stateName)
        {
            var state = await GetStateById(stateName);            

            return View(state);
        }


        // GET ALL STATES
        public async Task<IEnumerable<State>> GetAllStates()
        {
            IEnumerable<State> states = new List<State>();

            try
            {
                string json;
                HttpResponseMessage response = await _client.GetAsync("/api/states");
                Debug.WriteLine("checking response and get all states");
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    states = JsonConvert.DeserializeObject<IEnumerable<State>>(json);

                    //foreach (State state in states)
                    //{
                    //    // print each state
                    //    Debug.WriteLine(state.StateName);
                    //}
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return states;
        }

        // GET STATE BY ID
        public async Task<State> GetStateById(string stateName)
        {
            try
            {
                State state;
                HttpResponseMessage response = await _client.GetAsync($"/api/states/{stateName}");
                if (response.IsSuccessStatusCode)
                {
                    state = await response.Content.ReadAsAsync<State>();

                    // print the state details 
                    Debug.WriteLine(
                        $"\nSTATE NAME: {stateName} \n" +
                        $"STATE DETAILS:\n"
                        + state.Abbreviation + "\n"
                        + state.Capital + "\n"
                        + state.Population + "\n"
                        + state.Region + "\n"
                    );

                    return state;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        // POST - CREATE STATE
        public async Task CreateState()
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

                HttpResponseMessage response = await _client.PostAsJsonAsync("/api/states", state);
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
        public async Task UpdateState(string stateName)
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
                response = await _client.PutAsync($"/api/states/{stateName}", content);
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
        public async Task PatchState(string stateName, JsonPatchDocument<State> patchDocument)
        {
            try
            {
                // Serialize the JsonPatchDocument to JSON
                string json = JsonConvert.SerializeObject(patchDocument);
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Send the PATCH request
                HttpResponseMessage response = await _client.PatchAsync($"/api/states/{stateName}", content);

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
        public async Task DeleteState(string stateName)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync($"/api/states/{stateName}");
                Debug.WriteLine($"STATUS FROM DELETE: {response.StatusCode}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
