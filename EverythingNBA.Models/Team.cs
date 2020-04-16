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
            this.TitlesWon = new HashSet<Season>();
            this.Players = new HashSet<Player>();
            this.SeasonsStatistics = new HashSet<SeasonStatistic>();
            this.AwayGames = new HashSet<Game>();
            this.GamesWon = new HashSet<Game>();
            this.HomeGames = new HashSet<Game>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(MinTeamName), MaxLength(MaxTeamName)]
        public string Name { get; set; }

        public string AbbreviatedName { get; set; }

        public int? FullImageId { get; set; }

        public virtual CloudinaryImage FullImage { get; set; }

        public int? SmallImageId { get; set; }

        public virtual CloudinaryImage SmallImage { get; set; }

        [Required]
        public ConferenceType Conference { get; set; }

        [Required]
        public string Venue { get; set; }

        public string Instagram { get; set; }

        public string Twitter { get; set; }

        public string PrimaryColorHex { get; set; }

        public string SecondaryColorHex { get; set; }

        public virtual ICollection<SeasonStatistic> SeasonsStatistics { get; set; } = new List<SeasonStatistic>();
        public virtual ICollection<Game> AwayGames { get; set; }
        public virtual ICollection<Game> GamesWon { get; set; }
        public virtual ICollection<Game> HomeGames { get; set; }
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<Season> TitlesWon { get; set; }
    }
}
