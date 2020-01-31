namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Season
    {
        public Season()
        {
            this.AllStarTeams = new List<AllStarTeam>();
            this.Games = new List<Game>();
            this.SingleSeasonStatistics = new List<SeasonStatistic>();
        }

        public int Id { get; set; }

        [Required]
        public int Year { get; set; }

        
        public int? TitleWinnerId { get; set; }
        public virtual Team TitleWinner { get; set; }

        [Required]
        public int GamesPlayed { get; set; }


        public int? PlayoffId { get; set; }
        public virtual Playoff Playoff { get; set; }

        public virtual ICollection<AllStarTeam> AllStarTeams { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<SeasonStatistic> SingleSeasonStatistics { get; set; }
    }
}
