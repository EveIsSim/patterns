using System;
using System.Transactions;
using EveIsSim.AmbientTransaction.Core;
using Xunit;

namespace EveIsSim.AmbientTransaction.UnitTests;

public class TransactionTests
{
    private readonly TransactionScopeFactory _factory = new TransactionScopeFactory();

    [Fact]
    public void Create_Default_ShouldSuccess()
    {
        // Act
        using (var ts = _factory.CreateTransactionScope())
        {
            Assert.NotNull(Transaction.Current);

            ts.Complete();
        }

        Assert.Null(Transaction.Current);
    }

    [Fact]
    public void CreateTransactionScope_WithTimeout_ShouldSuccess()
    {
        // Arrange
        var timeout = TimeSpan.FromSeconds(30);

        // Act
        using (var ts = _factory.CreateTransactionScope())
        {
            Assert.NotNull(Transaction.Current);

            ts.Complete();
        }

        Assert.Null(Transaction.Current);
    }

    [Fact]
    public void CreateTransactionScope_WithIsolationLevel_ShouldSetCorrectIsolationLevel()
    {
        // Arrange
        var isolationLevel = IsolationLevel.ReadCommitted;
        var timeout = TimeSpan.FromSeconds(30);

        // Act
        using var ts = _factory.CreateTransactionScope(isolationLevel, timeout);

        // Assert
        Assert.NotNull(Transaction.Current);
        Assert.Equal(isolationLevel, Transaction.Current.IsolationLevel);
        ts.Complete();
    }

    [Fact]
    public void CreateTransactionScope_WithOption_ShouldSetAmbientTransaction()
    {
        // Arrange
        var option = TransactionScopeOption.RequiresNew;
        var isolationLevel = IsolationLevel.ReadCommitted;
        var timeout = TimeSpan.FromSeconds(30);

        // Act
        using var ts = _factory.CreateTransactionScope(option, isolationLevel, timeout);

        // Assert
        Assert.NotNull(Transaction.Current);
        Assert.Equal(isolationLevel, Transaction.Current.IsolationLevel);

        ts.Complete();
    }

    [Fact]
    public async Task CreateTransactionScope_AsyncFlow_ShouldFlowAmbientTransaction()
    {
        // Act
        using var ts = _factory.CreateTransactionScope();

        await Task.Delay(10);

        // Assert: Ambient-transaction should moved correctly
        Assert.NotNull(Transaction.Current);

        ts.Complete();

    }

    [Fact]
    public async Task Transaction_Should_NotCommit_WhenAsyncOperationFails()
    {
        // Arrange
        bool exceptionThrown = false;

        // Act
        try
        {
            using var ts = _factory.CreateTransactionScope();

            await SimulateAsyncErrorOperation();

            ts.Complete();
        }
        catch (InvalidOperationException)
        {
            exceptionThrown = true;
        }

        // Assert
        Assert.True(exceptionThrown);
        Assert.Null(Transaction.Current); // rollback
    }

    private async Task SimulateAsyncErrorOperation()
    {
        await Task.Delay(10);
        throw new InvalidOperationException("Simulated operation error");
    }
}
