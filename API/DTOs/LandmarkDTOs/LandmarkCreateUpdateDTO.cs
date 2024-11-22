namespace API.DTOs.LandmarkDTOs
{
    public class LandmarkCreateUpdateDTO
    {
        public string StateName { get; set; }
        public string LandmarkName { get; set; } // Sort Key
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
