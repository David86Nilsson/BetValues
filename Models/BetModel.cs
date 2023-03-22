using System.ComponentModel.DataAnnotations.Schema;

namespace BetValue.Models
{
    public class BetModel
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public GameModel Game { get; set; }
        public double Odds { get; set; }
        public string Bet { get; set; } = null!;
        public string? Result { get; set; }
        public double BetValue { get; set; }


    }
}
