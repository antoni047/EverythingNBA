namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    using EverythingNBA.Models.Enums;
    using EverythingNBA.Models.MappingTables;
    using static EverythingNBA.Models.Utilities.DataConstants;

    public class Player
    {
        public Player()
        {
            this.AllStarTeams = new HashSet<AllStarTeamsPlayers>();
            this.Awards = new HashSet<Award>();
            this.SingleGameStatistics = new HashSet<GameStatistic>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string LastName { get; set; }

        
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }

        public int? RookieYear { get; set; }

        [Required]
        [Range(MinAge, MaxAge)]
        public int Age { get; set; }

        [Required]
        [Range(MinHeight, MaxHeight)]
        public int Height { get; set; }

        [Required]
        [Range(MinWeight, MaxHeight)]
        public int Weight { get; set; }

        [Required]
        public PositionType Position { get; set; }

        public bool IsStarter { get; set; }

        public int? CloudinaryImageId { get; set; }

        public virtual CloudinaryImage CloudinaryImage { get; set; }

        [Required]
        [Range(MinShirtNumber, MaxShirtNumber)]
        public int ShirtNumber { get; set; }

        public string InstagramLink { get; set; }

        public string TwitterLink { get; set; }

        public virtual ICollection<Award> Awards { get; set; }

        public virtual ICollection<AllStarTeamsPlayers> AllStarTeams { get; set; }

        public virtual ICollection<GameStatistic> SingleGameStatistics { get; set; }
    }
}
