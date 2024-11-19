using API.DTOs.StateDTOs;
using API.Models;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Xml.XPath;

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

            // Use AutoMapper to map the entities to DTOs
            var stateSummaries = _mapper.Map<List<StateSummaryDTO>>(states);

            return Ok(stateSummaries);
        }

        // 2. Get state by id
        [HttpGet("{stateName}")]
        public async Task<ActionResult<StateDetailDTO>> GetStateById(string stateName)
        {
            // Logic to get a state by ID
            var state = await _stateRepository.GetStateByNameAsync(stateName);

            // If state is not found, return 404
            if (state == null) return NotFound();

            var stateDetail = _mapper.Map<StateDetailDTO>(state);

            // Return the state
            return Ok(stateDetail);
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
            return CreatedAtAction(nameof(GetStateById), new { stateName = createdState.StateName }, createdState);
        }
    
        // 4. Fully update a state
        [HttpPut("{stateName}")]
        public async Task<IActionResult> UpdateState(string stateName, [FromBody] StateCreateUpdateDTO updatedState)
        {
            // Handle model validation errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the state exists
            var existingState = await _stateRepository.GetStateByNameAsync(stateName);
            if (existingState == null)
            {
                return NotFound($"State with name '{stateName}' not found.");
            }

            // Ensure URL stateName matches the DTO stateName
            if (!string.Equals(stateName, updatedState.StateName, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("State name in URL does not match the state name in the body.");
            }

            // Map the DTO to the existing entity
            _mapper.Map(updatedState, existingState);

            // Call repository method to update the state
            await _stateRepository.UpdateStateAsync(existingState);

            // Return the updated state
            return Ok(existingState);
        }

        // 5. Partially update a state
        [HttpPatch("{stateName}")]
        public async Task<IActionResult> PatchState(string stateName, [FromBody] JsonPatchDocument<State> patchDocument)
        {
            if (string.IsNullOrEmpty(stateName) || patchDocument == null)
                return BadRequest("StateName and Patch document must be provided.");

            var existingState = await _stateRepository.GetStateByNameAsync(stateName);


            if (existingState == null)
                return NotFound($"State with name '{stateName}' not found.");

            // Directly apply the patch to the entity
            patchDocument.ApplyTo(existingState, error => ModelState.AddModelError(error.AffectedObject?.ToString() ?? "Unknown", error.ErrorMessage));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Save the updated state
            await _stateRepository.UpdateStateAsync(existingState);


            return Ok(existingState);
        }

        // 6. Delete a state
        [HttpDelete("{stateName}")]
        public async Task<string> DeleteState(string stateName)
        {
            string result = await _stateRepository.DeleteStateAsync(stateName);
            return result;
        }
    }
}
