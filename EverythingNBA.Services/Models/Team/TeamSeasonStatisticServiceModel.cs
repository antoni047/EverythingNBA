﻿namespace EverythingNBA.Services.Models
{
    public class TeamSeasonStatisticServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageURL { get; set; }

        public string Conference { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public string WinPercentage { get; set; }

        public int GamesPlayed { get; set; }

        public string LastTenGames { get; set; }
    }
}
