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

        Task<ICollection<AwardDetailsServiceModel>> GetSeasonAwardsAsync(int year);

        Task<string> GetAwardWinnerAsync(int seasonId, string awardType);

        Task<ICollection<PlayerAwardsServiceModel>> GetPlayerAwardsAsync(int playerId);

        Task EditAwardWinnerAsync(string winnerName, int awardId);

        Task<ICollection<AllAwardsServiceModel>> GetAllAwardsAsync();

        Task<AwardDetailsServiceModel> GetAwardDetails(int awardId);
    }
}
