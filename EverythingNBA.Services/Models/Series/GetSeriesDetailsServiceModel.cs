namespace EverythingNBA.Services.Models.Series
{
    using System.Collections.Generic;
    using EverythingNBA.Services.Models.Game;

    public class GetSeriesDetailsServiceModel
    {
        public int Id { get; set; }

        public int? Team1Id { get; set; }

        public int? Team2Id { get; set; }

        public int? WinnerGamesWon { get; set; }

        public int? LoserGamesWon { get; set; }

        public int MostPoints { get; set; }

        public int MostAssists { get; set; }

        public int MostRebounds { get; set; }

        public ICollection<GameOverviewServiceModel> Games { get; set; }
    }
}
