using System.Data;
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

    public async Task<AccountEntity?> GetOwner(int accountId, CancellationToken token)
    {
        var sql = "SELECT * FROM accounts WHERE id = @Id";

        using var connection = await CreateConnectionAsync();
        var cmd = new CommandDefinition(sql, new { Id = accountId }, cancellationToken: token);
        var result = await connection.QueryAsync<AccountEntity>(cmd);
        return result.FirstOrDefault();
    }

    public async Task<(bool IsSucces, string ErrMsg)> UpdateBalance(AccountEntity[] entities, CancellationToken token)
    {
        var tasks = entities.Select(x => UpdateBalance(x, token));
        var result = await Task.WhenAll(tasks);
        return (true, "");
    }

    private async Task<(bool IsSucces, string ErrMsg)> UpdateBalance(AccountEntity entity, CancellationToken token)
    {
        var sql = "UPDATE accounts SET owner_name = @OwnerName, balance = @Balance WHERE id = @Id";

        using var connection = await CreateConnectionAsync();
        var cmd = new CommandDefinition(sql, new { entity.OwnerName, entity.Balance, entity.Id }, cancellationToken: token);
        await connection.ExecuteAsync(cmd);
        return (true, "");
    }

    public async Task<(bool IsSucces, string ErrMsg)> AddLog(TransactionLogEntity entity, CancellationToken token)
    {
        var sql = @"
                INSERT INTO transaction_logs (source_account_id, destination_account_id, amount, timestamp)
                VALUES (@SourceAccountId, @DestinationAccountId, @Amount, @Timestamp)";

        using var connection = await CreateConnectionAsync();
        var cmd = new CommandDefinition(sql, new { entity.SourceAccountId, entity.DestinationAccountId, entity.Amount, entity.Timestamp }, cancellationToken: token);
        await connection.ExecuteAsync(cmd);
        return (true, "");
    }
}
