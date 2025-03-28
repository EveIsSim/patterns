using EveIsSim.AmbientTransaction.App.Repositories;
using EveIsSim.AmbientTransaction.App.Services.Models;
using EveIsSim.AmbientTransaction.Core;

namespace EveIsSim.AmbientTransaction.App.Services;


public class BankingService : IBankingService
{
    private readonly IBankingRepository _db;
    private readonly ITransactionScopeFactory _transactionScopeFactory;

    public BankingService(
            IBankingRepository db,
            ITransactionScopeFactory transactionScopeFactory
            )
    {
        _db = db;
        _transactionScopeFactory = transactionScopeFactory;
    }


    public async Task<(bool IsSuccess, string ErrorMsg)> Transfer(MoneyTransferDto r, CancellationToken token)
    {
        if (r.SourceAccountId == r.DestinationAccountId)
            return (false, "acount should be different");
        if (r.SourceAccountId <= 0 || r.DestinationAccountId <= 0)
            return (false, "acount_id should be positive");

        var source = await _db.GetOwner(r.SourceAccountId, token);
        if (source is null) return (false, "sourceAccountId does not exist");

        if (source.Balance < r.Amount)
            return (false, "there are not enough funds on the balance to transfer the specified amount");

        var destination = await _db.GetOwner(r.DestinationAccountId, token);
        if (destination is null) return (false, "destinationAccountId does not exist");

        source.Balance -= r.Amount;
        destination.Balance += r.Amount;


        using var ts = _transactionScopeFactory.CreateTransactionScope();

        await _db.UpdateBalance([source, destination], token);

        if (r.IsFailTransaction)
        {
            return (false, "Transaction rollback. Your operation should not be completed");
        }

        await _db.AddLog(r.ToLog(), token);

        ts.Complete();

        return (true, string.Empty);
    }

}
