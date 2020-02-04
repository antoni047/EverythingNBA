namespace EverythingNBA.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    using EverythingNBA.Services.Models.Playoff;

    public interface IPlayoffService
    {
        Task<int> AddPlayoffAsync(int? seasonId, int westernQuarterFinalFirstId, int westernQuarterFinalSecondId, int westernQuarterFinalThirdId, 
            int westernQuarterFinalFourthId, int westernSemiFinalFirstId, int westernSemiFinalSecondId, int westernFinalId, int easternQuarterFinalFirstId,
            int easternQuarterFinalSecondId,  int easternQuarterFinalThird, int easternQuarterFinalFourth, int easternSemiFinalFirst, 
            int easternSemiFinalSecondId, int easternFinalId, int finalId);

        Task<bool> DeletePlayoffAsync(int playoffId);

        Task<GetPlayoffServiceModel> GetPlayoffDetailsAsync(int playoffId);

        Task<GetPlayoffServiceModel> GetPlayoffBySeasonAsync(int seasonId);
    }
}
