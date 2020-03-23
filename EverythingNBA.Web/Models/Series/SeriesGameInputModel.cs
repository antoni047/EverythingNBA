namespace EverythingNBA.Web.Models.Series
{
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Web.Utilities.Enums;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static EverythingNBA.Models.Utilities.DataConstants;

    public class SeriesGameInputModel
    {
        public int Id { get; set; }

        public int SeriesId { get; set; }

        [Required]
        [EnumDataType(typeof(ConferenceType))]
        public string Conference { get; set; }

        [Required]
        [EnumDataType(typeof(PlayoffStageType))]
        public string Stage { get; set; }

        [Required]
        [Range(1, 4)]
        public int StageNumber { get; set; }

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

        public int GameNumber { get; set; }
    }
}
