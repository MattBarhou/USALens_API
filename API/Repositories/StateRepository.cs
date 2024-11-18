using Amazon.DynamoDBv2.DataModel;
using API.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace API.Repositories
{
    public class StateRepository : IStateRepository
    {
        private readonly IDynamoDBContext _context;

        //Constructor
        public StateRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        //Get all states
        public async Task<IEnumerable<State>> GetStatesAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<State>(conditions).GetRemainingAsync();
        }

        //Get state by id
        public async Task<State> GetStateByIdAsync(string id)
        {
            return await _context.LoadAsync<State>(id);
        }

        //Add a state
        public async Task<State> AddStateAsync(State state)
        {
             await _context.SaveAsync(state);
             return state;
        }

        //Update a state
        public async Task<State> UpdateStateAsync(string id, State state)
        {
            // get id
            state.StateName = id;

            // Save the state
            await _context.SaveAsync(state);
            return state;
        }

        //Patch a state
        public async Task<State> PatchStateAsync(string id, JsonPatchDocument<State> patchDocument)
        {
            var existingState = await GetStateByIdAsync(id);

            if (existingState == null)
            {
                throw new KeyNotFoundException($"State with the ID of {id} was not found");
            }

            // Apply the patch to the state
            patchDocument.ApplyTo(existingState);

            // Save the patched state
            await _context.SaveAsync(existingState);
            return existingState;
        }

        //Delete a state
        public async Task<string> DeleteStateAsync(string id)
        {
            await _context.DeleteAsync<State>(id);
            return "State Deleted Successfully";
        }

    }
}
