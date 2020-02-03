namespace EverythingNBA.Services.Models.AllStarTeam
{
    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;

    using System.Collections.Generic;

    public class GetAllStarTeamServiceModel
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string Type { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
