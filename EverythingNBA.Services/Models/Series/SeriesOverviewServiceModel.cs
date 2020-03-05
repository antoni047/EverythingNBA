namespace EverythingNBA.Services.Models.Series
{
    public class SeriesOverviewServiceModel
    {
        public string Team1Name { get; set; }

        public string Team2Name { get; set; }

        public int? Team1GamesWon { get; set; }

        public int? Team2GamesWon { get; set; }

        public string Stage { get; set; }

        public string Number { get; set; }

        public string Conference { get; set; }
    }
}
