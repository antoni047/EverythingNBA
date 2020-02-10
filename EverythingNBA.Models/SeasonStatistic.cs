using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EverythingNBA.Models
{
    public class SeasonStatistic
    {
        public int Id { get; set; }

        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int? SeasonId { get; set; }
        public Season Season { get; set; }

        [Required]
        public int Wins { get; set; }

        [Required]
        public int Losses { get; set; }
    }
}
