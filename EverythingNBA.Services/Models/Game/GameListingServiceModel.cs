namespace EverythingNBA.Services.Models.Game
{
    using System;
    using System.Collections.Generic;

    public class GameListingServiceModel
    {
        public ICollection<GameDetailsServiceModel> Games { get; set; }

        public ICollection<DateTime> Dates { get; set; }

        public int Total { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        public bool PreviousPageIsDisabled => this.CurrentPage == 1;

        public bool NextPageIsDisabled
        {
            get
            {
                var maxPage = Math.Ceiling(((double)this.Total) / 10);

                return maxPage == this.CurrentPage;
            }
        }
    }
}
