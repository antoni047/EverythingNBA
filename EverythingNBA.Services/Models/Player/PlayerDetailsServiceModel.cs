using EverythingNBA.Models;
using EverythingNBA.Services.Models.AllStarTeam;

using System.Collections.Generic;

namespace EverythingNBA.Services.Models.Player
{
    public class PlayerDetailsServiceModel
    {
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

        public ICollection<Award> Awards { get; set; }

        public ICollection<GetAllStarTeamServiceModel> AllStarTeams { get; set; }

        public ICollection<EverythingNBA.Models.GameStatistic> SingleGameStatistics { get; set; }
    }
}
