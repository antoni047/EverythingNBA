namespace EverythingNBA.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Game
    {
        public Game()
        {
            this.PlayerStats = new List<SingleGameStatistic>();
        }

        public int Id { get; set; }

        
        public int? SeasonId { get; set; }
        public Season Season { get; set; }

        
        public int? TeamHostId { get; set; }
        public Team TeamHost { get; set; }

        
        public int? Team2Id { get; set; }
        public Team Team2 { get; set; }

        
        public int? WinnerId { get; set; }
        public Team Winner { get; set; }

        public int? WinnerPoints { get; set; }
        public int? LoserPoints { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public ICollection<SingleGameStatistic> PlayerStats{ get; set; }
    }
}
