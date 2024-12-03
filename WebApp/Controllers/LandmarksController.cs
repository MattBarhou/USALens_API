using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LandmarksController : Controller
    {
        private readonly HttpClient _client;

        public LandmarksController(HttpClient client)
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
            IEnumerable<Landmark> landmarks = new List<Landmark>(); // Initialize to avoid null
            try
            {
                HttpResponseMessage response = await _client.GetAsync("/api/landmarks");

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(response.Content.ReadAsStringAsync());
                    string json = await response.Content.ReadAsStringAsync();
                    landmarks = JsonConvert.DeserializeObject<IEnumerable<Landmark>>(json);
                }
                else
                {
                    Debug.WriteLine($"API call failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Error fetching landmarks: {e.Message}");
            }

            Debug.WriteLine(landmarks);
            // Ensure a non-null object is always passed to the view
            return View(landmarks ?? new List<Landmark>());
        }

        // Show Landmark Details
        [HttpGet]
        public async Task<IActionResult> Details(string landmarkName)
        {
            if (string.IsNullOrEmpty(landmarkName))
            {
                return BadRequest("LandmarkName is required.");
            }

            var response = await _client.GetAsync($"/api/landmarks/{landmarkName}");
            if (response.IsSuccessStatusCode)
            {
                var landmark = JsonConvert.DeserializeObject<Landmark>(await response.Content.ReadAsStringAsync());
                return View(landmark); // Render the Details view with the landmark model
            }

            return NotFound($"Landmark {landmarkName} not found.");
        }

        // Update Landmark 
        [HttpPost]
        public async Task<IActionResult> UpdateLandmark(Landmark updatedLandmark)
        {
            if (updatedLandmark == null || string.IsNullOrEmpty(updatedLandmark.LandmarkName))
            {
                return BadRequest("Landmark data is invalid.");
            }

            try
            {
                var response = await _client.PutAsJsonAsync($"/api/landmarks/{updatedLandmark.LandmarkName}", updatedLandmark);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect after successful update
                }
                else
                {
                    // Log and handle API errors
                    Debug.WriteLine($"API Error: {response.StatusCode}");
                    return BadRequest("Failed to update landmark.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Patch Landmark
        [HttpPost]
        public async Task<IActionResult> PatchLandmark(string landmarkName, string? description, string? location)
        {
            if (string.IsNullOrEmpty(landmarkName))
            {
                return BadRequest("LandmarkName is required.");
            }

            // Create a JsonPatchDocument
            var patchDocument = new JsonPatchDocument<Landmark>();
            if (!string.IsNullOrEmpty(description))
            {
                patchDocument.Replace(s => s.Description, description);
            }

            if (!string.IsNullOrEmpty(location))
            {
                patchDocument.Replace(s => s.Location, location);
            }

            if (patchDocument.Operations.Count == 0)
            {
                return BadRequest("No fields to update.");
            }

            try
            {
                // Send the patch request to the API
                var response = await _client.PatchAsync(
                    $"/api/landmarks/{landmarkName}",
                    new StringContent(JsonConvert.SerializeObject(patchDocument), Encoding.UTF8, "application/json")
                );

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    Debug.WriteLine($"API Error: {response.StatusCode}");
                    return BadRequest("Failed to patch landmark.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        // Delete Landmark
        [HttpPost]
        public async Task<IActionResult> DeleteLandmark(string landmarkName)
        {
            if (string.IsNullOrEmpty(landmarkName))
            {
                return BadRequest("LandmarkName is required.");
            }

            try
            {
                var response = await _client.DeleteAsync($"/api/landmarks/{landmarkName}");
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index"); // Redirect after successful deletion
                }
                else
                {
                    // Log and handle API errors
                    Debug.WriteLine($"API Error: {response.StatusCode}");
                    return BadRequest("Failed to delete landmark.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        //// GET: LandmarksController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: LandmarksController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: LandmarksController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: LandmarksController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
