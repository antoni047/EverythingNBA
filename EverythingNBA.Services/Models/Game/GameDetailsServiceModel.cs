namespace EverythingNBA.Services.Models.Game
{
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.GameStatisticModels;

    using System;
    using System.Collections.Generic;

    public class GameDetailsServiceModel
    {
        public int Id { get; set; }

        public int? TeamHostId { get; set; }

        public int? Team2Id { get; set; }

        public bool IsFinished { get; set; }

        public int? TeamHostPoints { get; set; }
        public int? Team2Points { get; set; }

        public string Date { get; set; }

        public string Venue { get; set; }
    }
}
