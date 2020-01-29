namespace EverythingNBA.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using EverythingNBA.Models.Enums;
    using EverythingNBA.Models.MappingTables;
    using static EverythingNBA.Models.Utilities.DataConstants;

    public class Team
    {
        public Team()
        {
            this.TitlesWon = new List<Season>();
            this.Players = new List<Player>();
            this.SeasonsStatistics = new List<SeasonStatistic>();
            this.AwayGames = new List<Game>();
            this.GamesWon = new List<Game>();
            this.HomeGames = new List<Game>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(MinTeamName), MaxLength(MaxTeamName)]
        public string Name { get; set; }

        public int? CloudinaryImageId { get; set; }

        public virtual CloudinaryImage CloudinaryImage { get; set; }


        [Required]
        public ConferenceType Conference { get; set; }

        [Required]
        public string Venue { get; set; }

        public string Instagram { get; set; }

        public string Twitter { get; set; }

        public virtual ICollection<SeasonStatistic> SeasonsStatistics { get; set; }
        public virtual ICollection<Game> AwayGames { get; set; }
        public virtual ICollection<Game> GamesWon { get; set; }
        public virtual ICollection<Game> HomeGames { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Season> TitlesWon { get; set; }
    }
}
