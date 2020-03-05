namespace EverythingNBA.Services.Models.Player
{
    using System.Collections.Generic;

    public class PlayerAccomplishmentsListingServiceModel
    {
        public List<int> FirstAllNBATeams { get; set; }

        public List<int> SecondAllNBATeams { get; set; }

        public List<int> ThirdAllNBATeams { get; set; }

        public List<int> AllDefensiveTeams { get; set; }

        public List<int> AllRookieTeams { get; set; }

        public List<int> MVPs { get; set; }

        public List<int> FinalsMVPs { get; set; }

        public List<int> TopScorerTitles { get; set; }

        public List<int> DPOTYs { get; set; }

        public List<int> ROTYs { get; set; }

        public List<int> SixthMOTYs { get; set; }

        public List<int> MIPs { get; set; }
    }
}
