namespace EverythingNBA.Models
{
    using EverythingNBA.Models.Enums;
    using static EverythingNBA.Models.Utilities.DataConstants;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Award
    {
        public int Id { get; set; }

        [Required]
        public AwardType Name { get; set; }

        [Required]
        [Range(MinYear, MaxYear)]
        public int Year { get; set; }
        
        public int? WinnerId { get; set; }
        public virtual Player Winner { get; set; }

        public string WinnerTeamName { get; set; }

        public int? SeasonId { get; set; }

        public virtual Season Season { get; set; }
    }
}
