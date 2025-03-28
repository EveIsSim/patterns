using EveIsSim.AmbientTransaction.App.Services.Models;

namespace EveIsSim.AmbientTransaction.App.Services;

public interface IBankingService
{
    Task<(bool IsSuccess, string ErrorMsg)> Transfer(MoneyTransferDto r, CancellationToken token);
}
