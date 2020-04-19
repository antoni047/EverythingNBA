namespace EverythingNBA.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    public class Game
    {
        public Game()
        {
            this.PlayerStats = new HashSet<GameStatistic>();

        }

        public int Id { get; set; }


        public int? SeasonId { get; set; }
        public virtual Season Season { get; set; }


        public int? TeamHostId { get; set; }
        public virtual Team TeamHost { get; set; }


        public int? Team2Id { get; set; }
        public virtual Team Team2 { get; set; }

        public bool IsFinished { get; set; }

        public int? TeamHostPoints { get; set; }
        public int? Team2Points { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public bool IsPlayoffGame { get; set; } = false;

        public virtual ICollection<GameStatistic> PlayerStats { get; }
    }
}
