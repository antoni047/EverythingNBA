namespace EverythingNBA.Services.Models.Series
{
    public class SeriesOverviewServiceModel
    {
        public int Id { get; set; }

        public int PlayoffId { get; set; }

        public string Team1Name { get; set; }

        public int Team1StandingsPosition { get; set; }

        public string Team2Name { get; set; }

        public int Team2StandingsPosition { get; set; }

        public int? Team1GamesWon { get; set; }

        public int? Team2GamesWon { get; set; }

        public string Stage { get; set; }

        public int StageNumber { get; set; }

        public string Conference { get; set; }
    }
}
