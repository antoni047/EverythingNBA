namespace EverythingNBA.Web.Models.Home
{
    using Services.Models.Game;

    using System.Collections.Generic;

    public class IndexViewModel
    {
        public List<GameDetailsServiceModel> GamesToday { get; set; }

        public List<GameDetailsServiceModel> GamesTomorrow { get; set; }

        public List<GameDetailsServiceModel> GamesYesterday { get; set; }

        public List<ShortTeamStandingsViewModel> WesternTop8Standings { get; set; }

        public List<ShortTeamStandingsViewModel> EasternTop8Standings { get; set; }
    }
}
