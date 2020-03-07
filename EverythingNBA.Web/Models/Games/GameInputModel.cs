using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EverythingNBA.Web.Models.Games
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    using static EverythingNBA.Models.Utilities.DataConstants;

    public class GameInputModel
    {
        [Required]
        [MinLength(MinTeamName)]
        [MaxLength(MaxTeamName)]
        public string TeamHostName { get; set; }

        [Required]
        [MinLength(MinTeamName)]
        [MaxLength(MaxTeamName)]
        public string Team2Name { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        [Range(0, 200)]
        public int TeamHostPoints { get; set; }

        [Required]
        [Range(0, 200)]
        public int Team2Points { get; set; }

        [Required]
        public bool IsFinished { get; set; }
    }
}
