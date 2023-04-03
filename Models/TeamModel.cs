using BetValue.Enums;
using System.ComponentModel.DataAnnotations;

namespace BetValue.Models;

public class TeamModel
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    //public List<string> ShortNames { get; set; } = new();
    public string Pitch { get; set; }
    public TeamModel(string Name)
    {
        this.Name = Name;
        if (Enum.IsDefined(typeof(PlasticPitch), this.Name.Replace(' ', '_')))
        {
            Pitch = "Plast";
        }
        else
        {
            Pitch = "Grass";
        }
    }

}