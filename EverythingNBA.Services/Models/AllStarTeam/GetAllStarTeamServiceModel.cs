namespace EverythingNBA.Services.Models.AllStarTeam
{
    using System.Collections.Generic;

    using EverythingNBA.Models;

    public class GetAllStarTeamServiceModel
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public int Year { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
