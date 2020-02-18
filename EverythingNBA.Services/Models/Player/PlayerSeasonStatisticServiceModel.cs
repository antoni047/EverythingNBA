namespace EverythingNBA.Services.Models.Player
{
    public class PlayerSeasonStatisticServiceModel
    {
        public int PlayerId { get; set; }

        public int SeasonId { get; set; }

        public string AveragePoints { get; set; }

        public string AverageAssists { get; set; }

        public string AverageRebounds { get; set; }

        public string AverageBlocks { get; set; }

        public string AverageSteals { get; set; }

        public double AverageFieldGoalPercentage { get; set; }

        public double AverageFreeThrowPercentage { get; set; }

        public double AverageThreePercentage { get; set; }
    }
}
