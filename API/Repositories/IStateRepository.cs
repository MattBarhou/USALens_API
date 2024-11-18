using API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Repositories
{
    public interface IStateRepository
    {
        Task<IEnumerable<State>> GetStatesAsync();

        Task<State> GetStateByIdAsync(string id);

        Task<State> AddStateAsync(State state);

        Task<State> PatchStateAsync(string id, JsonPatchDocument<State> patchDocument);

        Task<State> UpdateStateAsync(string id, State state);

        Task<string> DeleteStateAsync(string id);
    }
}
