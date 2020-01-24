using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EverythingNBA.Models
{
    public class SingleSeasonStatistic
    {
        public int Id { get; set; }

        
        public int? SeasonId { get; set; }
        public Season Season { get; set; }

        [Required]
        public int Wins { get; set; }

        [Required]
        public int Losses { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int WinPercentage {
            get { return (Wins / (Wins + Losses)) * 100;}
}
    }
}
