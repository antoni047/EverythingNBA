using System;
namespace EverythingNBA.Web.Models.AllStarTeams
{
    using System.ComponentModel.DataAnnotations;

    using static EverythingNBA.Models.Utilities.DataConstants;

    public class EditAllStarTeamInputModel
    {
        [Required]
        [Range(MinYear, MaxYear)]
        public int Year { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
