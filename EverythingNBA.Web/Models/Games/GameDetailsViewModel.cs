﻿namespace EverythingNBA.Web.Models.Games
{
    using System.Collections.Generic;

    using Services.Models.GameStatisticModels;

    public class GameDetailsViewModel
    {
        public string TeamHostName { get; set; }

        public string Team2Name { get; set; }

        public int TeamHostPoints { get; set; }

        public int Team2Points { get; set; }

        public string TeamHostSeasonStatistic { get; set; }

        public string Team2SeasonStatistic { get; set; }

        public string Date { get; set; }

        public string Venue { get; set; }

        public ICollection<PlayerGameStatisticServiceModel> TeamHostPlayerStats { get; set; }

        public ICollection<PlayerGameStatisticServiceModel> Team2PlayerStats { get; set; }
    }
}