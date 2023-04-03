using System.ComponentModel.DataAnnotations.Schema;

namespace BetValue.Models

{
    public class SerieModel
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public int LongestTeamNameLength { get; set; } = 0;

        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public LeagueModel League { get; set; }
        public string? TxtSchedule { get; set; }
        public string? TxtChances { get; set; }
    }
}