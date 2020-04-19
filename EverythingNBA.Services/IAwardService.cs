namespace EverythingNBA.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.Award;

    public interface IAwardService
    {
        Task<string> AddAwardAsync(string name, int year, string winnerName, string winnerTeamName);

        Task<bool> DeleteAwardAsync(int awardId);

        Task<ICollection<AwardDetailsServiceModel>> GetSeasonAwardsAsync(int year);

        Task<string> GetAwardWinnerAsync(int seasonId, string awardType);

        Task<ICollection<PlayerAwardsServiceModel>> GetPlayerAwardsAsync(int playerId);

        Task EditAwardWinnerAsync(string winnerName, int awardId);

        Task<ICollection<AllAwardsServiceModel>> GetAllAwardsAsync();

        Task<AwardDetailsServiceModel> GetAwardDetails(int awardId);
    }
}
