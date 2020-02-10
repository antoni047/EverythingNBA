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
            //this.TitlesWon = new List<Season>();
            //this.Players = new List<Player>();
            //this.SeasonsStatistics = new HashSet<SeasonStatistic>();
            this.AwayGames = new List<Game>();
            this.GamesWon = new List<Game>();
            this.HomeGames = new List<Game>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(MinTeamName), MaxLength(MaxTeamName)]
        public string Name { get; set; }

        public string AbbreviatedName { get; set; }

        public int? CloudinaryImageId { get; set; }

        public virtual CloudinaryImage CloudinaryImage { get; set; }


        [Required]
        public ConferenceType Conference { get; set; }

        [Required]
        public string Venue { get; set; }

        public string Instagram { get; set; }

        public string Twitter { get; set; }

        public ICollection<SeasonStatistic> SeasonsStatistics { get; set; } = new List<SeasonStatistic>();
        public ICollection<Game> AwayGames { get; set; }
        public ICollection<Game> GamesWon { get; set; }
        public ICollection<Game> HomeGames { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
        public ICollection<Season> TitlesWon { get; set; } = new List<Season>();
    }
}
