using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3.Model;
using API.Helpers;
using API.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Cryptography.X509Certificates;

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
            return await _context.ScanAsync<State>(new List<ScanCondition>()).GetRemainingAsync();
        }

        //Get state by id
        public async Task<State> GetStateByNameAsync(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
            {
                throw new ArgumentException("StateName must be provided.");
            }

            //Convert to uppercase
            var normalizedStateName = stateName;

            // Query using the partition key (StateName)
            var query = new DynamoDBOperationConfig
            {
                QueryFilter = new List<ScanCondition>
                {
                    new ScanCondition("StateName", ScanOperator.Equal, normalizedStateName)
                }
            };

            var results = await _context.QueryAsync<State>(normalizedStateName, query).GetRemainingAsync();
            return results.FirstOrDefault(); // Return the first matching state or null
        }

        //Add a state
        public async Task<State> AddStateAsync(State state)
        {
             await _context.SaveAsync(state);
             return state;
        }

        //Update a state
        public async Task<State> UpdateStateAsync(State state)
        {
            ArgumentNullException.ThrowIfNull(state);

            // Save the state
            await _context.SaveAsync(state);
            return state;
        }

        //Patch a state
        public async Task<State> PatchStateAsync(string stateName, JsonPatchDocument<State> patchDocument)
        {
            var existingState = await GetStateByNameAsync(stateName);

            if (existingState == null)
                throw new KeyNotFoundException($"State with the name '{stateName}' was not found.");

            patchDocument.ApplyTo(existingState);

            await _context.SaveAsync(existingState);

            return existingState;
        }

        //Delete a state
        public async Task<string> DeleteStateAsync(string stateName)
        {
            if (string.IsNullOrEmpty(stateName))
            {
                throw new ArgumentException("StateName must be provided.");
            }

            var normalizedState = stateName;

            // Query to get the item by StateName
            var queryConfig = new DynamoDBOperationConfig
            {
                QueryFilter = new List<ScanCondition>
                {
                    new ScanCondition("StateName", ScanOperator.Equal, normalizedState)
                }
            };

            var results = await _context.QueryAsync<State>(normalizedState, queryConfig).GetRemainingAsync();

            var state = results.FirstOrDefault();
            if (state == null)
            {
                throw new KeyNotFoundException($"State with name '{normalizedState}' not found.");
            }

            // Delete the state using both Partition Key and Sort Key
            await _context.DeleteAsync<State>(state.StateName, state.Abbreviation);
            return "State Deleted Successfully";
        }

    }
}
