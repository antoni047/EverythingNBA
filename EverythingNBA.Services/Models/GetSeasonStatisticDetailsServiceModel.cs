using System;
using System.Collections.Generic;
using System.Text;

namespace EverythingNBA.Services.Models
{
    public class GetSeasonStatisticDetailsServiceModel
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public int? SeasonId { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }
    }
}
