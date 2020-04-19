namespace EverythingNBA.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EverythingNBA.Models.MappingTables;
    using EverythingNBA.Models.Enums;
    using static EverythingNBA.Models.Utilities.DataConstants;

    public class AllStarTeam
    {
        public AllStarTeam()
        {
            this.Players = new HashSet<AllStarTeamsPlayers>();
        }

        public int Id { get; set; }
        [Required]
        [Range(MinYear, MaxYear)]
        public int Year { get; set; }

        [Required]
        public AllStarTeamType Type { get; set; }

        public virtual ICollection<AllStarTeamsPlayers> Players { get; }
    }
}
