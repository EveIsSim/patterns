using System.Transactions;

namespace EveIsSim.AmbientTransaction.Core;


public class TransactionScopeFactory : ITransactionScopeFactory
{
    // Only for demonstration
    public TransactionScope CreateTransactionScopeWithLogging()
    {
        Console.WriteLine($" => [TxFactory] Creating TransactionScope");
        Console.WriteLine($" => [TxFactory] Thread: {Thread.CurrentThread.ManagedThreadId}");

        var scope = CreateTransactionScope(IsolationLevel.ReadCommitted, timeout: TimeSpan.FromSeconds(30));

        Console.WriteLine($" => [TxFactory] TransactionScope created. Transaction.Current ID: {Transaction.Current?.TransactionInformation.LocalIdentifier}");

        return scope;
    }

    public TransactionScope CreateTransactionScope()
    {
        return CreateTransactionScope(IsolationLevel.ReadCommitted, timeout: TimeSpan.FromSeconds(30));
    }

    public TransactionScope CreateTransactionScope(TimeSpan timeout)
    {
        return CreateTransactionScope(IsolationLevel.ReadCommitted, timeout);
    }

    public TransactionScope CreateTransactionScope(IsolationLevel isolationLevel, TimeSpan timeout)
    {
        return CreateTransactionScope(TransactionScopeOption.Required, isolationLevel, timeout);
    }

    public TransactionScope CreateTransactionScope(TransactionScopeOption transactionScopeOption, IsolationLevel isolationLevel, TimeSpan timeout)
    {
        return new TransactionScope(
                transactionScopeOption,
                new TransactionOptions { IsolationLevel = isolationLevel, Timeout = timeout },
                TransactionScopeAsyncFlowOption.Enabled
                );
    }
}
