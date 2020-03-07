namespace EverythingNBA.Web.Models.Games
{
    using System.ComponentModel.DataAnnotations;

    using static EverythingNBA.Models.Utilities.DataConstants;

    public class GameStatisticInputModel
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        [Required]
        [Range(MinMinutesPlayed, MaxMinutesPlayed)]
        public int MinutesPlayed { get; set; }

        //Basic
        [Required]
        [Range(MinimumStat, MaximumPoints)]
        public double Points { get; set; }

        [Required]
        [Range(MinimumStat, MaximumGeneralStat)]
        public double Assists { get; set; }

        [Required]
        [Range(MinimumStat, MaximumGeneralStat)]
        public double Rebounds { get; set; }


        //Advanced
        [Range(MinimumStat, MaximumGeneralStat)]
        public double Steals { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double Blocks { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double FieldGoalsMade { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double FieldGoalAttempts { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double ThreeMade { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double ThreeAttempts { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double FreeThrowsMade { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double FreeThrowAttempts { get; set; }
    }
}
