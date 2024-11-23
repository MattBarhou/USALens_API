using Amazon.DynamoDBv2.DataModel;

namespace API.Models
{
    [DynamoDBTable("Landmarks")]
    public class Landmark
    {
        [DynamoDBHashKey] // Partition key
        public required string StateName { get; set; }

        [DynamoDBRangeKey] // Sort key 
        public required string LandmarkName { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public string Location { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }
    }
}
