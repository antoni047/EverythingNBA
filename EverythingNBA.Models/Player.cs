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
            this.AllStarTeams = new List<AllStarTeamsPlayers>();
            this.Awards = new List<Award>();
            this.SingleGameStatistics = new List<GameStatistic>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(NameMinLenght), MaxLength(NameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(NameMinLenght), MaxLength(NameMaxLenght)]
        public string LastName { get; set; }

        
        public int? TeamId { get; set; }
        public Team Team { get; set; }

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

        public CloudinaryImage CloudinaryImage { get; set; }

        [Required]
        [Range(MinShirtNumber, MaxShirtNumber)]
        public int ShirtNumber { get; set; }

        public string InstagramLink { get; set; }

        public string TwitterLink { get; set; }

        public ICollection<Award> Awards { get; set; }
        public ICollection<AllStarTeamsPlayers> AllStarTeams { get; set; }

        public ICollection<GameStatistic> SingleGameStatistics { get; set; }

        //Current Season Stats 

        //Basic Stats
        [Range(MinimumStat, MaximumPoints)]
        public double CurrentAvereagePoints { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double CurrentAverageAssists { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double CurrentAverageRebounds { get; set; }

        //Advanced Stats
        [Range(MinimumStat, MaximumGeneralStat)]
        public double CurrentAverageBlocks { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double CurrentAverageSteals { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double CurrentFieldGoalPercentage { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double CurrentThreePercentage { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double CurrentFreeThrowPercentage { get; set; }



        //Career Stats
        [Range(MinimumStat, MaximumGeneralStat)]
        public double CareerAvereagePoints { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double CareerAverageAssists { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double CareerAverageRebounds { get; set; }



        [Range(MinimumStat, MaximumGeneralStat)]
        public double CareerAverageBlocks { get; set; }

        [Range(MinimumStat, MaximumGeneralStat)]
        public double CareerAverageSteals { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double CareerAverageFieldGoalPercentage { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double CareerAverageThreePercentage { get; set; }

        [Range(MinimumPercentage, MaximumPercentage)]
        public double CareerAverageFreeThrowPercentage { get; set; }

    }
}
