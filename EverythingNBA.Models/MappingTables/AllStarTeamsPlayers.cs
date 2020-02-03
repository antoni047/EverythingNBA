using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EverythingNBA.Models.MappingTables
{
    public class AllStarTeamsPlayers
    {
        [Required]
        public int PlayerId { get; set; }
        public virtual ICollection<Player> Players { get; set; }

        [Required]
        public int AllStarTeamId { get; set; }
        public virtual ICollection<AllStarTeam> AllStarTeam { get; set; }
    }
}
