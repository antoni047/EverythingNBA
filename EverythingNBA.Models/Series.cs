namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Series
    {
        public int Id { get; set; }

        
        public int? Team1Id { get; set; }
        public Team Team1 { get; set; }

        
        public int? Team2Id { get; set; }
        public Team Team2 { get; set; }

        
        public int? WinnerId { get; set; }
        public Team Winner { get; set; }

        public int? WinnerGamesWon { get; set; }
        public int? LoserGamesWon { get; set; }

        public int? Game1Id { get; set; }
        public Game Game1 { get; set; }

        public int? Game2Id { get; set; }
        public Game Game2 { get; set; }

        public int? Game3Id { get; set; }
        public Game Game3 { get; set; }

        public int? Game4Id { get; set; }
        public Game Game4 { get; set; }

        public int? Game5Id { get; set; }
        public Game Game5 { get; set; }

        public int? Game6Id { get; set; }
        public Game Game6 { get; set; }

        public int? Game7Id { get; set; }
        public Game Game7 { get; set; }
    }
}
