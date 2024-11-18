using API.DTOs.StateDTOs;
using API.Models;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatesController : Controller
    {
        //Mapper
        private readonly IMapper _mapper;

        //Repository
        private readonly IStateRepository _stateRepository;

        //Constructor with DI
        public StatesController(IMapper mapper, IStateRepository stateRepository)
        {
            _mapper = mapper;
            _stateRepository = stateRepository;
        }


        // 1. Get all states
        [HttpGet]
        public async Task<ActionResult<List<StateSummaryDTO>>> GetAllStates()
        {
            // Logic to get all states

            var states = await _stateRepository.GetStatesAsync();
            return Ok(states);
        }

        // 2. Get state by id
        [HttpGet("{id}")]
        public async Task<ActionResult<StateDetailDTO>> GetStateById(string id)
        {
            // Logic to get a state by ID
            var state = await _stateRepository.GetStateByIdAsync(id);

            // If state is not found, return 404
            if (state == null) return NotFound();

            // Return the state
            return Ok(state);
        }

        // 3. Create a state
        [HttpPost]
        public async Task<IActionResult> CreateState([FromBody] StateCreateUpdateDTO newState)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Use AutoMapper to map the DTO to the entity
            var stateEntity = _mapper.Map<State>(newState);

            // Call repository method to add the state
            var createdState = await _stateRepository.AddStateAsync(stateEntity);

            // Return the created state 
            return CreatedAtAction(nameof(GetStateById), new { id = createdState.StateName }, createdState);
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
        public async Task<string> DeleteState(string id)
        {
        string result = await _stateRepository.DeleteStateAsync(id);
            return result;
        }
    }
}
