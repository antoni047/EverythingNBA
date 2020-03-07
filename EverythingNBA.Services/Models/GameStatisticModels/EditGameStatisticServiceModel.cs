namespace EverythingNBA.Services.Models.GameStatisticModels
{
    public class EditGameStatisticServiceModel
    {
        public string PlayerName { get; set; }

        public bool IsPlayerStarter { get; set; }

        public int MinutesPlayed { get; set; }

        public int Points { get; set; }

        public int Assists { get; set; }

        public int Rebounds { get; set; }

        public int Steals { get; set; }

        public int Blocks { get; set; }

        public double FieldGoalsMade { get; set; }

        public double FieldGoalAttempts { get; set; }

        public double ThreeMade { get; set; }

        public double ThreeAttempts { get; set; }

        public double FreeThrowsMade { get; set; }

        public double FreeThrowAttempts { get; set; }
    }
}
