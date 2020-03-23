namespace EverythingNBA.Web.Models.Series
{
    using System.ComponentModel.DataAnnotations;

    using static EverythingNBA.Models.Utilities.DataConstants;
    using EverythingNBA.Models.Enums;
    using Web.Utilities.Enums;

    public class SeriesInputModel
    {
        public int Id { get; set; }

        public int PlayoffId { get; set; }

        [MaxLength(MaxTeamName)]
        [MinLength(MinTeamName)]
        public string Team1Name { get; set; }

        [MaxLength(MaxTeamName)]
        [MinLength(MinTeamName)]
        public string Team2Name { get; set; }

        [Range(0, 4)]
        public int Team1GamesWon { get; set; }

        [Range(0, 4)]
        public int Team2GamesWon { get; set; }

        [Required]
        [EnumDataType(typeof(ConferenceType))]
        public string Conference { get; set; }

        [Required]
        [EnumDataType(typeof(PlayoffStageType))]
        public string Stage { get; set; }

        [Required]
        [Range(1, 4)]
        public int StageNumber { get; set; }
    }
}
