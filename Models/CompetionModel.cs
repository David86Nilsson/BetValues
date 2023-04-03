using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetValue.Models
{
    public class CompetionModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        [ForeignKey(nameof(Country))]
        public int? CountryId { get; set; }
        public CountryModel? Country { get; set; }
    }
}
