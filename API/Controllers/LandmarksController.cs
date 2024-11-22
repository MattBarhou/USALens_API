using API.DTOs.LandmarkDTOs;
using API.Models;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LandmarksController : Controller
    {
        private readonly IMapper _mapper;

        private readonly ILandmarkRepository _landmarkRepository;

        public LandmarksController(IMapper mapper, ILandmarkRepository landmarkRepository)
        {
            _mapper = mapper;
            _landmarkRepository = landmarkRepository;
        }

        //Get all landmarks
        [HttpGet]
        public async Task<ActionResult<List<LandmarkSummaryDTO>>> GetAllLandmarks()
        {
            var landmarks = await _landmarkRepository.GetLandmarksAsync();

            var landmarkSummaries = _mapper.Map<List<LandmarkSummaryDTO>>(landmarks);

            return Ok(landmarkSummaries);
        }

        //Get landmark by name
        [HttpGet("{landmarkName}")]
        public async Task<ActionResult<LandmarkDetailDTO>> GetLandmarkByName(string landmarkName)
        {
            var landmark = await _landmarkRepository.GetLandmarkByNameAsync(landmarkName);

            if (landmark == null) return NotFound();

            var landmarkDetail = _mapper.Map<LandmarkDetailDTO>(landmark);

            return Ok(landmarkDetail);
        }

        //Add a landmark
        [HttpPost]
        public async Task<IActionResult> AddLandmark([FromBody] LandmarkCreateUpdateDTO landmarkCreateDTO)
        {
  
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var landmark = _mapper.Map<Landmark>(landmarkCreateDTO);

            var newLandmark = await _landmarkRepository.AddLandmarkAsync(landmark);

            return CreatedAtAction(nameof(GetLandmarkByName), new { landmarkName = newLandmark.LandmarkName }, newLandmark);
        }

        //Update a landmark
        [HttpPut("{landmarkName}")]
        public async Task<IActionResult> UpdateLandmark(string landmarkName, [FromBody] LandmarkCreateUpdateDTO landmarkUpdateDTO)
        {
            var landmark = await _landmarkRepository.GetLandmarkByNameAsync(landmarkName);

            if (landmark == null) return NotFound();

            _mapper.Map(landmarkUpdateDTO, landmark);

            var updatedLandmark = await _landmarkRepository.UpdateLandmarkAsync(landmark);

            var landmarkDetail = _mapper.Map<LandmarkDetailDTO>(updatedLandmark);

            return Ok(landmarkDetail);
        }

        //Patch a landmark
        [HttpPatch("{landmarkName}")]
        public async Task<IActionResult> PatchLandmark(string landmarkName, [FromBody] JsonPatchDocument<Landmark> patchDocument)
        {
            var landmark = await _landmarkRepository.GetLandmarkByNameAsync(landmarkName);

            if (landmark == null) return NotFound();

            patchDocument.ApplyTo(landmark, error => ModelState.AddModelError(error.AffectedObject?.ToString() ?? "Unknown", error.ErrorMessage));

            var updatedLandmark = await _landmarkRepository.UpdateLandmarkAsync(landmark);

            return Ok(updatedLandmark);
        }

        //Delete a landmark
        [HttpDelete("{landmarkName}")]
        public async Task<string> DeleteLandmark(string landmarkName)
        {
            var landmark = await _landmarkRepository.GetLandmarkByNameAsync(landmarkName);

            if (landmark == null) return "Landmark not found.";

            string result = await _landmarkRepository.DeleteLandmarkAsync(landmarkName);

            return result;
        }
    }
}
