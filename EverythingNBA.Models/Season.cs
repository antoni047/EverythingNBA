namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System;

    public class Season
    {
        public Season()
        {
            this.AllStarTeams = new HashSet<AllStarTeam>();
            this.Games = new HashSet<Game>();
            this.SeasonStatistics = new HashSet<SeasonStatistic>();
            this.Awards = new HashSet<Award>();
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

        public DateTime SeasonStartDate { get; set; }

        public DateTime SeasonEndDate { get; set; }

        public virtual ICollection<AllStarTeam> AllStarTeams { get; set; }

        public virtual ICollection<Award> Awards { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        public virtual ICollection<SeasonStatistic> SeasonStatistics { get; set; }
    }
}
