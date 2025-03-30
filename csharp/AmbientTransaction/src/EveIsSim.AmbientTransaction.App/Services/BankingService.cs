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

        var sourceResult = await _db.GetOwner(r.SourceAccountId, token);
        if (sourceResult.ErrMsg != null) return (false, sourceResult.ErrMsg);
        if (sourceResult.Data is null) return (false, "sourceAccountId does not exist");

        if (sourceResult.Data!.Balance < r.Amount)
            return (false, "there are not enough funds on the balance to transfer the specified amount");

        var destinationResult = await _db.GetOwner(r.DestinationAccountId, token);
        if (destinationResult.ErrMsg != null) return (false, destinationResult.ErrMsg);
        if (destinationResult.Data is null) return (false, "destinationAccountId does not exist");

        sourceResult.Data.Balance -= r.Amount;
        destinationResult.Data.Balance += r.Amount;


        using var ts = _transactionScopeFactory.CreateTransactionScope();

        await _db.UpdateBalance(new[] { sourceResult.Data, destinationResult.Data }, token);

        if (r.IsFailTransaction)
        {
            return (false, "Transaction rollback. Your operation should not be completed");
        }

        await _db.AddLog(r.ToLog(), token);

        ts.Complete();

        return (true, string.Empty);
    }

}
