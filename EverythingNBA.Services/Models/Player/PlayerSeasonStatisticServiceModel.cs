﻿namespace EverythingNBA.Services.Models.Player
{
    public class PlayerSeasonStatisticServiceModel
    {
        public int PlayerId { get; set; }

        public int SeasonId { get; set; }

        public double AveragePoints { get; set; }

        public double AverageAssists { get; set; }

        public double AverageRebounds { get; set; }

        public double AverageBlocks { get; set; }

        public double AverageSteals { get; set; }

        public double AverageFieldGoalPercentage { get; set; }

        public double AverageFreeThrowPercentage { get; set; }

        public double AverageThreePercentage { get; set; }
    }
}
