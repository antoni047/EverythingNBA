namespace EverythingNBA.Services.Models.Player
{
    public class PlayerRecentGamesListingServiceModel
    {
        public string TeamHostName { get; set; }

        public string Team2Name { get; set; }

        public string Date { get; set; }

        public int TeamHostPoints { get; set; }

        public int Team2Points { get; set; }

        public int Points { get; set; }

        public int Assists { get; set; }

        public int Rebounds { get; set; }
    }
}
