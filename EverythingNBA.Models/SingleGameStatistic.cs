namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;
    using static EverythingNBA.Models.Utilities.DataConstants;
    public class SingleGameStatistic
    {
        public int Id { get; set; }

        
        public int GameId { get; set; }
        public Game Game { get; set; }

        
        public int PlayerId { get; set; }
        public Player Player { get; set; }

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
        public double FieldgoalAttempts { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double Fielgoalpercentage { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double ThreeMade { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double ThreeAttempts { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double ThreePercentage { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double FreeThrowsMade { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double FreethrowAttempts { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double FreeThrowPercentage { get; set; }

    }
}
