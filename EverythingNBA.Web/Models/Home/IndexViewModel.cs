namespace EverythingNBA.Web.Models.Home
{
    using Services.Models.Game;

    using System.Collections.Generic;

    public class IndexViewModel
    {
        public ICollection<GameDetailsServiceModel> GamesToday { get; set; }

        public ICollection<GameDetailsServiceModel> GamesTomorrow { get; set; }

        public ICollection<GameDetailsServiceModel> GamesYesterday { get; set; }

        public ICollection<ShortTeamStandingsViewModel> WesternTop8Standings { get; set; }

        public ICollection<ShortTeamStandingsViewModel> EasternTop8Standings { get; set; }
    }
}
