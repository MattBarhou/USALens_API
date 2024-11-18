using API.DTOs.StateDTOs;

namespace API.DTOs.RegionDTOs
{
    public class RegionDetailDTO
    {
        public string RegionName { get; set; }
        public List<StateSummaryDTO> States { get; set; }
    }
}
