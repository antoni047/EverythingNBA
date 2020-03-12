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

        public string Stage { get; set; }

        public string Conference { get; set; }

        public TopStatsServiceModel TopStats { get; set; }

        public ICollection<GameDetailsServiceModel> Games { get; set; }
    }
}
