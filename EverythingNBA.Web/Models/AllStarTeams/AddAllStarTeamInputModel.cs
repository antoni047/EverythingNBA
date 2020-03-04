namespace EverythingNBA.Web.Models.AllStarTeams
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    using static EverythingNBA.Models.Utilities.DataConstants;

    public class AddAllStarTeamInputModel
    {
        [Required]
        [Range(MinYear, MaxYear)]
        public int Year { get; set; }

        [Required]
        public string Type { get; set; }

        public ICollection<string> PlayerNames { get; set; }
    }
}
