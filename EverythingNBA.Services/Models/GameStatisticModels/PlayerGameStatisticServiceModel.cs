namespace EverythingNBA.Services.Models.GameStatisticModels
{
    public class PlayerGameStatisticServiceModel
    {
        public string PlayerName { get; set; }

        public bool IsPlayerStarter { get; set; }

        public int MinutesPlayed { get; set; }

        public int Points { get; set; }

        public int Assists { get; set; }

        public int Rebounds { get; set; }

        public int Steals { get; set; }

        public int Blocks { get; set; }

        public double FieldGoalPercentage { get; set; }

        public double FreeThrowPercentage { get; set; }

        public double ThreePercentage { get; set; }
    }
}
