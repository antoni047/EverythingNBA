namespace EverythingNBA.Services
{
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Award;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAwardService
    {
        Task<int> AddAwardAsync(string name, int year, string winnerName, string winnerTeamName);

        Task<bool> DeleteAwardAsync(int awardId);

        Task<ICollection<AwardDetailsServiceModel>> GetSeasonAwardsAsync(int seasonId);

        Task<string> GetAwardWinnerAsync(int seasonId, string awardType);

        Task<ICollection<string>> GetPlayerAwardsAsync (int playerId);

        Task EditAwardWinnerAsync(string winnerName, int awardId);

        ICollection<AllAwardsServiceModel> GetAllAwards();

        Task<AwardDetailsServiceModel> GetAwardDetails(int awardId);
    }
}
