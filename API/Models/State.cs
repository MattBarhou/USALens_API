using Amazon.DynamoDBv2.DataModel;

namespace API.Models
{
    [DynamoDBTable("States")]
    public class State
    {
        [DynamoDBHashKey] // Partition key
        public required string StateName { get; set; }

        [DynamoDBRangeKey] // Sort key 
        public required string Abbreviation { get; set; }

        [DynamoDBProperty]
        public string Capital { get; set; }

        [DynamoDBProperty]
        public int Population { get; set; }

        [DynamoDBProperty]
        public double Area { get; set; }

        [DynamoDBProperty]
        public string Region { get; set; }

        [DynamoDBProperty]
        public List<string> TimeZones { get; set; }

        [DynamoDBProperty]
        public string FlagUrl { get; set; }
    }
}
