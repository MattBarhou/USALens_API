using API.DTOs.StateDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : Controller
    {
        // 1. Get all states
        [HttpGet]
        public async Task<ActionResult<List<StateSummaryDTO>>> GetAllStates()
        {
            // Logic to get all states
            var states = await _stateService.GetAllStatesAsync();
            return Ok(states);
        }

        // 2. Get state by id
        [HttpGet("{id}")]
        public async Task<ActionResult<StateDetailDTO>> GetStateById(string id)
        {
            var state = await _stateService.GetStateByIdAsync(id);
            if (state == null) return NotFound();
            return Ok(state);
        }

        // 3. Create a state
        [HttpPost]
        public async Task<IActionResult> CreateState([FromBody] StateCreateUpdateDTO newState)
        {
            await _stateService.CreateStateAsync(newState);
            return CreatedAtAction(nameof(GetStateById), new { id = newState.StateName }, newState);
        }

        // 4. Fully update a state
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateState(string id, [FromBody] StateCreateUpdateDTO updatedState)
        {
            // Logic to update a state by ID
            return NoContent();
        }

        // 5. Partially update a state
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchState(string id, [FromBody] JsonPatchDocument<StateCreateUpdateDTO> patchDoc)
        {
            // Logic to partially update a state
            return NoContent();
        }

        // 6. Delete a state
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(string id)
        {
            // Logic to delete a state by ID
            return NoContent();
        }
    }
}
