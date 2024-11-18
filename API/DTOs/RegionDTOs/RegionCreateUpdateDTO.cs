namespace API.DTOs.RegionDTOs
{
    public class RegionCreateUpdateDTO
    {
        public string RegionName { get; set; }
        public List<string> StateNames { get; set; }
    }
}
