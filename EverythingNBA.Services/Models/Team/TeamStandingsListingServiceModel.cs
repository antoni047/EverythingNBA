namespace EverythingNBA.Services.Models.Team
{
    using System.Collections.Generic;

    public class TeamStandingsListingServiceModel
    {
        public ICollection<TeamSeasonStatisticServiceModel> EasternStandings { get; set; }

        public ICollection<TeamSeasonStatisticServiceModel> WesternStandings { get; set; }
    }
}
