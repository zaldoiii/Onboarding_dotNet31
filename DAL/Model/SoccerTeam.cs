using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class SoccerTeam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeamId { get; set; }

        [ForeignKey("CountryId")]
        public int CountryId { get; set; }

        public string TeamName { get; set; }

        public string TeamGroup { get; set; }

        public int PointsScored { get; set; }

        public SoccerTeam()
        {
        }
        public SoccerTeam(int _TeamId, int _CountryId, string _TeamName, string _TeamGroup, int _PointsScored)
        {
            this.TeamId = _TeamId;
            this.CountryId = _CountryId;
            this.TeamName = _TeamName;
            this.TeamGroup = _TeamGroup;
            this.PointsScored = _PointsScored;
        }
    }
}
