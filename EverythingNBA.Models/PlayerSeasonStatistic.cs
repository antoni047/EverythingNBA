namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;
    using static EverythingNBA.Models.Utilities.DataConstants;

    public class PlayerSeasonStatistic
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; }

        //All Stats Are Averages!
        [Range(MinimumStat, MaximumPoints)]
        public double Points { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double Assists { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double AverageRebounds { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double Blocks { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double Steals { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double FieldGoalPercentage { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double ThreePercentage { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double FreeThrowPercentage { get; set; }
    }
}
