﻿namespace EverythingNBA.Services.Models.Game
{
    using System;
    using System.Collections.Generic;

    public class GameOverviewServiceModel
    {
        public int Id { get; set; }

        public int? TeamHostId { get; set; }

        public int? Team2Id { get; set; }

        public bool IsFinished { get; set; }

        public int? TeamHostPoints { get; set; }
        public int? Team2Points { get; set; }

        public DateTime Date { get; set; }
    }
}