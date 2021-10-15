using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Model
{
    public class SoccerCountry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public string CountryAbbrev { get; set; }

        public int Population { get; set; }

        [JsonIgnore]
        public List<SoccerTeam> Teams { get; set; }

        public SoccerCountry()
        {
        }
        public SoccerCountry(int _CountryId, string _CountryName, string _CountryAbbrev, int _Population)
        {
            this.CountryId = _CountryId;
            this.CountryName = _CountryName;
            this.CountryAbbrev = _CountryAbbrev;
            this.Population = _Population;
        }
    }
}
