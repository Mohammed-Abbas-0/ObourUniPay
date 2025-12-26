using Obour_Uni_Pay.Models;

namespace Obour_Uni_Pay.Services
{
    public interface IQueueService
    {
        Task<QueueTurn> GenerateTurnAsync(string barcode);
        Task<QueueTurn?> GetTurnByIdAsync(int turnId);
        Task<List<QueueTurn>> GetDailyQueueAsync();
        Task<QueueTurn?> GetCurrentTurnAsync();
        Task<TurnStatus> UpdateTurnStatusAsync(int turnId, TurnStatus status);
        Task ResetQueueAsync();
    }
}
