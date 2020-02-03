using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EverythingNBA.Models.MappingTables
{
    public class AllStarTeamsPlayers
    {
        [Required]
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        [Required]
        public int AllStarTeamId { get; set; }
        public AllStarTeam AllStarTeam { get; set; }
    }
}
