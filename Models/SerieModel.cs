using System.ComponentModel.DataAnnotations.Schema;

namespace BetValue.Models

{
    public class SerieModel
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public int maxTeamNameLength { get; set; } = 0;

        [ForeignKey(nameof(League))]
        public int LeagueId { get; set; }
        public LeagueModel League { get; set; }
        public List<SerieMemberModel> SerieMembers { get; set; } = new();
        public List<GameModel> Games { get; set; } = new();
        //public List<Team> Teams { get; set; } = new();
        //public List<GameModel> GamesToGuess { get; set; } = new();
        public int nbrOfGames { get; set; }
        public int omg { get; set; } = 1;
        public int NbrOfTeams { get; set; }
        public int hGoals { get; set; } = 0;
        public int aGoals { get; set; } = 0;
        //public string[] lines { get; set; }
        public string? TxtSchedule { get; set; }
        public string? TxtChances { get; set; }
        public SerieModel()
        {
        }
    }
}