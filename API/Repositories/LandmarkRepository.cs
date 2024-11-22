using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.DTOs.LandmarkDTOs;
using API.Helpers;
using API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace API.Repositories
{
    public class LandmarkRepository : ILandmarkRepository
    {
        IDynamoDBContext _context;

        public LandmarkRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        //Get all landmarks
        public async Task<IEnumerable<Landmark>> GetLandmarksAsync()
        {
            return await _context.ScanAsync<Landmark>(new List<ScanCondition>()).GetRemainingAsync();

        }

        // Get landmark by name
        public async Task<Landmark> GetLandmarkByNameAsync(string landmarkName)
        {
            if (string.IsNullOrEmpty(landmarkName))
            {
                throw new ArgumentException("LandmarkName must be provided.");
            }

            // Normalize the LandmarkName (capitalize or trim if needed)
            var normalizedLandmarkName = Helper.CapitalizeLandmarkName(landmarkName);

            // Query using the sort key (LandmarkName)
            var query = new DynamoDBOperationConfig
            {
                QueryFilter = new List<ScanCondition>
                {
                    new ScanCondition("LandmarkName", ScanOperator.Equal, normalizedLandmarkName)
                }
            };

            // Perform the scan or query for the landmark
            var results = await _context.ScanAsync<Landmark>(new List<ScanCondition>
            {
                new ScanCondition("LandmarkName", ScanOperator.Equal, normalizedLandmarkName)
            }).GetRemainingAsync();

            return results.FirstOrDefault(); // Return the first matching landmark or null
        }
    
        
        // Add a landmark
        public async Task<Landmark> AddLandmarkAsync(Landmark newLandmark)
        {
            ArgumentNullException.ThrowIfNull(newLandmark);

            await _context.SaveAsync(newLandmark);
            return newLandmark;
        }

        // Update a landmark
        public async Task<Landmark> UpdateLandmarkAsync(Landmark updateLandmark)    
        {
            ArgumentNullException.ThrowIfNull(updateLandmark);

            await _context.SaveAsync(updateLandmark);
            return updateLandmark;
        }

        // Patch a landmark
        public async Task<Landmark> PatchLandmarkAsync(string landmarkName, JsonPatchDocument<Landmark> patchDocument)
        {
            var existingLandmark = await GetLandmarkByNameAsync(landmarkName);

            if (existingLandmark == null)
                throw new KeyNotFoundException($"Landmark with the name '{existingLandmark}' was not found.");

            patchDocument.ApplyTo(existingLandmark);

            await _context.SaveAsync(existingLandmark);

            return existingLandmark;
        }

        // Delete a landmark
        public async Task<string> DeleteLandmarkAsync(string landmarkName)
        {
            var landmark = await GetLandmarkByNameAsync(landmarkName);

            ArgumentNullException.ThrowIfNull(landmark);

            await _context.DeleteAsync(landmark);

            return "Landmark deleted successfully.";
        }
    }
}
