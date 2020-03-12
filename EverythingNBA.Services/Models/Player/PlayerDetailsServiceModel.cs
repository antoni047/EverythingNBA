namespace EverythingNBA.Services.Models.Player
{
    using System.Collections.Generic;

    public class PlayerDetailsServiceModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CurrentTeam { get; set; }

        public int? RookieYear { get; set; }

        public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public string Position { get; set; }

        public bool IsStarter { get; set; }

        public int? CloudinaryImageId { get; set; }

        public int ShirtNumber { get; set; }

        public string InstagramLink { get; set; }

        public string TwitterLink { get; set; }

        public PlayerCareerStatisticServiceModel CareerStatistics { get; set; }

        public PlayerSeasonStatisticServiceModel SeasonStatistics { get; set; }

        public ICollection<PlayerRecentGamesListingServiceModel> RecentGames { get; set; }

        //public ICollection<PlayerPastTeamsListingServiceModel> PastTeams { get; set; }
    }
}
