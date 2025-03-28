using EveIsSim.AmbientTransaction.App.Repositories.Models;

namespace EveIsSim.AmbientTransaction.App.Repositories;

public interface IBankingRepository
{
    Task<AccountEntity?> GetOwner(int accountId, CancellationToken token);
    Task<(bool IsSucces, string ErrMsg)> UpdateBalance(AccountEntity[] entities, CancellationToken token);
    Task<(bool IsSucces, string ErrMsg)> AddLog(TransactionLogEntity entity, CancellationToken token);
}
