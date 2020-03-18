namespace EverythingNBA.Services.Models.Team
{
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Player;
    using Services.Models.SeasonStatistic;
    using Services.Models.Game;

    using System.Collections.Generic;

    public class GetTeamDetailsServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AbbreviatedName { get; set; }

        public string FullImageURL { get; set; }

        public string Conference { get; set; }

        public string Venue { get; set; }

        public string Instagram { get; set; }

        public string Twitter { get; set; }

        public ICollection<SeasonStatisticOverviewServiceModel> CurrentSeasonStatistic { get; set; }

        public ICollection<TeamGameOverviewServiceModel> Last9Games { get; set; }

        public ICollection<TeamPlayerOverviewServiceModel> Players { get; set; }

        public string TitlesWon { get; set; }
    }
}
