using API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Repositories
{
    public interface ILandmarkRepository
    {
        Task<IEnumerable<Landmark>> GetLandmarksAsync();

        Task<Landmark> GetLandmarkByNameAsync(string landmarkName);

        Task<Landmark> AddLandmarkAsync(Landmark landmark);

        Task<Landmark> UpdateLandmarkAsync(Landmark landmark);

        Task<Landmark> PatchLandmarkAsync(string landmarkName, JsonPatchDocument<Landmark> patchDocument);

        Task<string> DeleteLandmarkAsync(string landmarkName);      
    }
}
