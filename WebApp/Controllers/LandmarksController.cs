using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
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
                    Console.WriteLine(response.Content.ReadAsStringAsync());
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
                Debug.WriteLine($"Error fetching states: {e.Message}");
            }

            Console.WriteLine(landmarks);
            // Ensure a non-null object is always passed to the view
            return View(landmarks ?? new List<Landmark>());
        }

        // GET: LandmarksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LandmarksController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LandmarksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LandmarksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LandmarksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LandmarksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LandmarksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
