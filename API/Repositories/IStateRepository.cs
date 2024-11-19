using API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Repositories
{
    public interface IStateRepository
    {
        Task<IEnumerable<State>> GetStatesAsync();

        Task<State> GetStateByNameAsync(string stateName);

        Task<State> AddStateAsync(State state);

        Task<State> PatchStateAsync(string stateName, JsonPatchDocument<State> patchDocument);

        Task<State> UpdateStateAsync(State state);

        Task<string> DeleteStateAsync(string stateName);
    }
}
