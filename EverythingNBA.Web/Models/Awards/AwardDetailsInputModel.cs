namespace EverythingNBA.Web.Models.Awards
{
    using System.ComponentModel.DataAnnotations;

    using static EverythingNBA.Models.Utilities.DataConstants;

    public class AwardDetailsInputModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(MaxTeamName)]
        [MinLength(MinTeamName)]
        public string Winner { get; set; }

        [Required]
        public string Type { get; set; }

        public string WinnerTeam { get; set; }

        [Required]
        [Range(1950, 3000)]
        public int Year { get; set; }
    }
}
