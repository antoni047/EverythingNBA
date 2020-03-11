namespace EverythingNBA.Web.Models.Players
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    using static EverythingNBA.Models.Utilities.DataConstants;

    public class PlayerInputModel
    {
        [Required]
        [MaxLength(NameMaxLenght)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(NameMaxLenght)]
        public string LastName { get; set; }

        public string Team { get; set; }

        [Range(MinYear, MaxYear)]
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
        public string Position { get; set; }

        public bool IsStarter { get; set; }

        public IFormFile Image { get; set; }

        [Required]
        [Range(MinShirtNumber, MaxShirtNumber)]
        public int ShirtNumber { get; set; }

        public string InstagramLink { get; set; }

        public string TwitterLink { get; set; }
    }
}
