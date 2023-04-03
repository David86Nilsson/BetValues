namespace BetValue.Models
{
    public class LeagueModel : CompetionModel
    {
        public string? UrlSchedule { get; set; }
        public string? UrlOdds { get; set; }
        public string? UrlChances { get; set; }
        public string? TxtOdds { get; set; }
        public LeagueModel(string Name) : base()
        {
            base.Name = Name;
            TxtOdds = Name + "Odds.txt";
        }
    }
}
