using Amazon.DynamoDBv2.DataModel;

namespace WebApp.Models
{
    public class State
    {
        public required string StateName { get; set; }

        public required string Abbreviation { get; set; }

        public string Capital { get; set; }

        public int Population { get; set; }

        public double Area { get; set; }

        public string Region { get; set; }

        public List<string> TimeZones { get; set; }

        public string FlagUrl { get; set; }
    }
}
