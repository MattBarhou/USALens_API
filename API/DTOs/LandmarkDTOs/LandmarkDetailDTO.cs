using API.DTOs.StateDTOs;

namespace API.DTOs.LandmarkDTOs
{
    public class LandmarkDetailDTO
    {
        public required string StateName { get; set; }    // Partition Key
        public required string LandmarkName { get; set; } // Sort Key
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
