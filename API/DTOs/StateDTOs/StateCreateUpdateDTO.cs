using System.ComponentModel.DataAnnotations;

namespace API.DTOs.StateDTOs
{
    public class StateCreateUpdateDTO
    {
        public required string StateName { get; set; }

        [StringLength(2, MinimumLength = 2)] // Abbreviation should always be 2 characters
        public required string Abbreviation { get; set; }

        public required string Capital { get; set; }

        [Range(1, int.MaxValue)] // Population must be greater than 0
        public required int Population { get; set; }

        public required double? Area { get; set; }

        public required string? Region { get; set; }

        public required List<string>? TimeZones { get; set; }
    }
}
