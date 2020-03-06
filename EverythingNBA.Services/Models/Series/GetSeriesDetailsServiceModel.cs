namespace EverythingNBA.Services.Models.Series
{
    using System.Collections.Generic;
    using EverythingNBA.Services.Models.Game;

    public class GetSeriesDetailsServiceModel
    {
        public int Id { get; set; }

        public int PlayoffId { get; set; }

        public string Team1Name { get; set; }

        public string Team2Name { get; set; }

        public int? Team1GamesWon { get; set; }

        public int? Team2GamesWon { get; set; }

        public int MostPoints { get; set; }

        public string MostPointsPlayerName { get; set; }

        public int MostAssists { get; set; }

        public string MostAssistsPlayerName { get; set; }

        public int MostRebounds { get; set; }

        public string MostReboundsPlayerName { get; set; }

        public string Stage { get; set; }

        public string Conference { get; set; }

        public ICollection<GameOverviewServiceModel> Games { get; set; }
    }
}
