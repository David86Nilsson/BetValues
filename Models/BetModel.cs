using System.ComponentModel.DataAnnotations.Schema;

namespace BetValue.Models
{
    public class BetModel
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Odds))]
        public int OddsId { get; set; }
        public OddsModel Odds { get; set; }
        public double PlayedOdds { get; set; }
        public string Bet { get; set; } = null!;
        public string? Result { get; set; }
        public double BetValue { get; set; }


    }
}
