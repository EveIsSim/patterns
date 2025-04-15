using System.Data;
using System.Transactions;
using Dapper;
using EveIsSim.AmbientTransaction.App.Repositories.Models;
using Npgsql;

namespace EveIsSim.AmbientTransaction.App.Repositories;

public class BankingRepository : IBankingRepository
{
    private readonly string _connectionString;

    public BankingRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<(AccountEntity? Data, string? ErrMsg)> GetOwner(int accountId, CancellationToken token)
    {
        var sql = "SELECT * FROM accounts WHERE id = @Id";

        try
        {
            using var connection = await CreateConnectionAsync();
            var cmd = new CommandDefinition(sql, new { Id = accountId }, cancellationToken: token);
            var result = await connection.QueryAsync<AccountEntity>(cmd);
            return (result.FirstOrDefault(), null);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    public async Task<(bool IsSucces, string ErrMsg)> UpdateBalance(AccountEntity[] entities, CancellationToken token)
    {
        try
        {
            foreach (var entity in entities)
            {
                var result = await UpdateBalance(entity, token);
                if (!result.IsSucces)
                    return result;
            }
            return (true, "");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private async Task<(bool IsSucces, string ErrMsg)> UpdateBalance(AccountEntity entity, CancellationToken token)
    {
        var sql = "UPDATE accounts SET owner_name = @OwnerName, balance = @Balance WHERE id = @Id";

        try
        {
            // check Transaction
            Console.WriteLine($" => => [DB] UpdateBalanca: UserId: {entity.Id}. Before OpenAsync. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($" => => [DB] UpdateBalanca: UserId: {entity.Id}. Transaction.Current: {Transaction.Current?.TransactionInformation.LocalIdentifier}");
            //
            using var connection = await CreateConnectionAsync();

            // check Transaction
            Console.WriteLine($" => => [DB] UpdateBalanca: UserId: UserId: {entity.Id}. After OpenAsync. Still in Transaction: {Transaction.Current?.TransactionInformation.LocalIdentifier}");
            //

            var cmd = new CommandDefinition(sql, new { entity.OwnerName, entity.Balance, entity.Id }, cancellationToken: token);
            await connection.ExecuteAsync(cmd);
            return (true, "");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public async Task<(bool IsSucces, string ErrMsg)> AddLog(TransactionLogEntity entity, CancellationToken token)
    {
        var sql = @"
                INSERT INTO transaction_logs (source_account_id, destination_account_id, amount, timestamp)
                VALUES (@SourceAccountId, @DestinationAccountId, @Amount, @Timestamp)";

        try
        {
            // check Transaction
            Console.WriteLine($" => => [DB] AddLog: UserFrom: {entity.SourceAccountId} -> UserTo: {entity.DestinationAccountId}. Before OpenAsync. Thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($" => => [DB] AddLog: UserFrom: {entity.SourceAccountId} -> UserTo: {entity.DestinationAccountId}. Transaction.Current: {Transaction.Current?.TransactionInformation.LocalIdentifier}");
            //
            using var connection = await CreateConnectionAsync();
            // check Transaction
            Console.WriteLine($" => => [DB] AddLog: UserFrom: {entity.SourceAccountId} -> UserTo: {entity.DestinationAccountId}. After OpenAsync. Still in Transaction: {Transaction.Current?.TransactionInformation.LocalIdentifier}");
            //
            var cmd = new CommandDefinition(sql, new { entity.SourceAccountId, entity.DestinationAccountId, entity.Amount, entity.Timestamp }, cancellationToken: token);
            await connection.ExecuteAsync(cmd);
            return (true, "");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
