using EverythingNBA.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverythingNBA.Services.Models
{
    public class GetSeasonDetailsServiceModel
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public int? TitleWinnerId { get; set; }

        public int GamesPlayed { get; set; }

        public int? PlayoffId { get; set; }

        //public virtual ICollection<AllStarTeam> AllStarTeams { get; set; }

        //public virtual ICollection<Award> Awards { get; set; }

        //public virtual ICollection<SeasonStatistic> SingleSeasonStatistics { get; set; }

        public string BestSeed { get; set; }

        public string WorstSeed { get; set; }

        public string TopScorer { get; set; }

        public string MVP { get; set; }

        public string DPOTY { get; set; }

        public string ROTY { get; set; }

        public string SixthMOTY { get; set; }

        public string FinalsMVP { get; set; }

        public string MIP { get; set; }

        public int FirstAllNBATeam { get; set; }

        public int SecondAllNBATeam { get; set; }

        public int ThirdAllNBATeam { get; set; }

        public int AllDefenesiveTeam { get; set; }

        public int AllRookieTeam { get; set; }
    }
}
