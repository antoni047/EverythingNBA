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

        public CloudinaryImage CloudinaryImage { get; set; }


        [Required]
        public ConferenceType Conference { get; set; }

        [Required]
        public string Venue { get; set; }

        public ICollection<SeasonStatistic> SeasonsStatistics { get; set; }
        public ICollection<Game> AwayGames { get; set; }
        public ICollection<Game> GamesWon { get; set; }
        public ICollection<Game> HomeGames { get; set; }
        public ICollection<Player> Players { get; set; }
        public ICollection<Season> TitlesWon { get; set; }
    }
}
