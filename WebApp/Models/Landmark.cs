using Amazon.DynamoDBv2.DataModel;

namespace WebApp.Models
{
    public class Landmark
    {
        public required string StateName { get; set; }
        public required string LandmarkName { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
