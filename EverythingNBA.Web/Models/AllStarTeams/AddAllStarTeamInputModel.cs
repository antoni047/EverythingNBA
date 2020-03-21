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

        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
        public string ThirdPlayer { get; set; }
        public string FourthPayers { get; set; }
        public string FifthPlayer { get; set; }

    }
}
