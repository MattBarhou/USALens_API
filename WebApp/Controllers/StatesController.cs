using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Headers;
using WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;


namespace WebApp.Controllers
{
    public class StatesController :  Controller
    {
        private readonly HttpClient _client;

        public StatesController(HttpClient client)
        {
            _client = client;
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7185")
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // index view
        public async Task<IActionResult> Index()
        {
            IEnumerable<State> states = new List<State>(); // Initialize to avoid null

            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/states");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.Content.ReadAsStringAsync());
                    string json = await response.Content.ReadAsStringAsync();
                    states = JsonConvert.DeserializeObject<IEnumerable<State>>(json);
                }
                else
                {
                    Debug.WriteLine($"API call failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error fetching states: {e.Message}");
            }

            Console.WriteLine(states);
            // Ensure a non-null object is always passed to the view
            return View(states ?? new List<State>());
        }

        //// details view

        //[Route("Home/Details/{stateName}")]
        //public async Task<IActionResult> Details(string stateName)
        //{
        //    var state = await GetStateById(stateName);

        //    return View(state);
        //}

        // details view
        [Route("States/EditedDetails")]
        public async Task<IActionResult> EditedDetails(State state)
        {
            return View(state);
        }


        // edit view
        [Route("States/Edit/{stateName}")]
        public async Task<IActionResult> Edit(string stateName, [Bind("StateName,Abbreviation,Capital,Population,Area,Region,TimeZones,FlagUrl")] State state)
        {
            Debug.WriteLine("ATTEMPT TO EDIT STATE >>>>>>>>>>>>>>>>>>>>>>>" + stateName);

            if (ModelState.IsValid)
            {
                await UpdateState(state);
                Debug.WriteLine("STATE UPDATED >>>>>>>>>>>>>>>>>>>>>>>" + stateName);
                return RedirectToAction(nameof(EditedDetails), state);
            }
            return View(state);
        }


        /// =====================  SERVICES  ===================== \\\

        // GET ALL STATES
        public async Task<IEnumerable<State>> GetAllStates()
        {
            IEnumerable<State> states = new List<State>();

            try
            {
                string json;
                HttpResponseMessage response = await _client.GetAsync("/api/states");
                //Debug.WriteLine("checking response and get all states");
                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    states = JsonConvert.DeserializeObject<IEnumerable<State>>(json);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return states;
        }

        // Show State Details
        [HttpGet]
        public async Task<IActionResult> Details(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
            {
                return BadRequest("StateName is required.");
            }

            var response = await _client.GetAsync($"/api/states/{stateName}");
            if (response.IsSuccessStatusCode)
            {
                var state = JsonConvert.DeserializeObject<State>(await response.Content.ReadAsStringAsync());
                return View(state); // Render the Details view with the state model
            }

            return NotFound($"State {stateName} not found.");
        }


        // Update State 
        [HttpPost]
        public async Task<IActionResult> UpdateState(State updatedState)
        {
            if (updatedState == null || string.IsNullOrEmpty(updatedState.StateName))
            {
                return BadRequest("State data is invalid.");
            }

            try
            {
                var response = await _client.PutAsJsonAsync($"/api/states/{updatedState.StateName}", updatedState);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect after successful update
                }
                else
                {
                    // Log and handle API errors
                    Console.WriteLine($"API Error: {response.StatusCode}");
                    return BadRequest("Failed to update state.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Patch State
        [HttpPost]
        public async Task<IActionResult> PatchState(string stateName, string? capital, int? population)
        {
            if (string.IsNullOrEmpty(stateName))
            {
                return BadRequest("StateName is required.");
            }

            // Create a JsonPatchDocument
            var patchDocument = new JsonPatchDocument<State>();
            if (!string.IsNullOrEmpty(capital))
            {
                patchDocument.Replace(s => s.Capital, capital);
            }

            if (population.HasValue)
            {
                patchDocument.Replace(s => s.Population, population.Value);
            }

            if (patchDocument.Operations.Count == 0)
            {
                return BadRequest("No fields to update.");
            }

            try
            {
                // Send the patch request to the API
                var response = await _client.PatchAsync(
                    $"/api/states/{stateName}",
                    new StringContent(JsonConvert.SerializeObject(patchDocument), Encoding.UTF8, "application/json")
                );

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Console.WriteLine($"API Error: {response.StatusCode}");
                    return BadRequest("Failed to patch state.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Delete State
        [HttpPost]
        public async Task<IActionResult> DeleteState(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
            {
                return BadRequest("StateName is required.");
            }

            try
            {
                var response = await _client.DeleteAsync($"/api/states/{stateName}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect after successful deletion
                }
                else
                {
                    // Log and handle API errors
                    Console.WriteLine($"API Error: {response.StatusCode}");
                    return BadRequest("Failed to delete state.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }        
    }
}
