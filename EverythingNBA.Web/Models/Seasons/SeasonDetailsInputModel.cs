namespace EverythingNBA.Web.Models.Seasons
{
    using System.ComponentModel.DataAnnotations;

    public class SeasonDetailsInputModel
    {
        public int Id { get; set; }

        [Required]
        [Range(1950, 3000)]
        public int Year { get; set; }

        public string TitleWinnerName { get; set; }

        [Required]
        [Range(0, 82)]
        public int GamesPlayed { get; set; }
    }
}
