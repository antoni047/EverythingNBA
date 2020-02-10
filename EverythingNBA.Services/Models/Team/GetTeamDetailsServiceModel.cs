namespace EverythingNBA.Services.Models.Team
{
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Player;

    using System.Collections.Generic;

    public class GetTeamDetailsServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AbbreviatedName { get; set; }

        public int? CloudinaryImageId { get; set; }

        public string Conference { get; set; }

        public string Venue { get; set; }

        public string Instagram { get; set; }

        public string Twitter { get; set; }

        public SeasonStatistic CurrentSeasonStatistic { get; set; }

        public ICollection<Game> CurrentSeasonGames { get; set; }

        public ICollection<PlayerOverviewServiceModel> Players { get; set; }

        public ICollection<int> TitlesWon { get; set; }
    }
}
