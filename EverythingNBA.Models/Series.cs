namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Series
    {
        public int Id { get; set; }

        public int PlayoffId { get; set; }
        public Playoff Playoff { get; set; }

        [Required]
        public string Conference { get; set; }

        [Required]
        public string Stage { get; set; }

        [Required]
        public int StageNumber { get; set; }


        public int? Team1Id { get; set; }
        public virtual Team Team1 { get; set; }

        public int Team1StandingsPosition { get; set; }

        public int? Team2Id { get; set; }
        public virtual Team Team2 { get; set; }

        public int Team2StandingsPosition { get; set; }

        public int? Team1GamesWon { get; set; }
        public int? Team2GamesWon { get; set; }

        public int? Game1Id { get; set; }
        public virtual Game Game1 { get; set; }

        public int? Game2Id { get; set; }
        public virtual Game Game2 { get; set; }

        public int? Game3Id { get; set; }
        public virtual Game Game3 { get; set; }

        public int? Game4Id { get; set; }
        public virtual Game Game4 { get; set; }

        public int? Game5Id { get; set; }
        public virtual Game Game5 { get; set; }

        public int? Game6Id { get; set; }
        public virtual Game Game6 { get; set; }

        public int? Game7Id { get; set; }
        public virtual Game Game7 { get; set; }
    }
}
