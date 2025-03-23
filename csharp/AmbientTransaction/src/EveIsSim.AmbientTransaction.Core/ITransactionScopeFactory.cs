using System.Transactions;

namespace EveIsSim.AmbientTransaction.Core;


public interface ITransactionScopeFactory
{
    TransactionScope CreateTransactionScope();
    TransactionScope CreateTransactionScope(TimeSpan timeout);
    TransactionScope CreateTransactionScope(IsolationLevel isolationLevel, TimeSpan timeout);
    TransactionScope CreateTransactionScope(TransactionScopeOption transactionScopeOption, IsolationLevel isolationLevel, TimeSpan timeout);
}
