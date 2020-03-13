namespace EverythingNBA.Web.Models.Teams
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    using static EverythingNBA.Models.Utilities.DataConstants;

    public class TeamInputModel
    {
        [Required]
        [MinLength(MinTeamName), MaxLength(MaxTeamName)]
        public string Name { get; set; }

        [Required]
        public string AbbreviatedName { get; set; }

        public IFormFile FullImage { get; set; }

        public IFormFile SmallImage { get; set; }

        [Required]
        public string Conference { get; set; }

        [Required]
        public string Venue { get; set; }

        public string Instagram { get; set; }

        public string Twitter { get; set; }
    }
}
