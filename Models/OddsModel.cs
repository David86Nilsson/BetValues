using System.ComponentModel.DataAnnotations.Schema;

namespace BetValue.Models
{
    public class OddsModel
    {
        public int Id { get; set; }
        public string? Operator { get; set; }
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public GameModel Game { get; set; } = null!;
        public double HomeWin { get; set; }
        public double Draw { get; set; }
        public double AwayWin { get; set; }
        public double FavoriteOdds { get; set; }
        public double HomeWinDNB { get; set; }
        public double AwayWinDNB { get; set; }
        public double FavoriteDNB { get; set; }
    }
}
