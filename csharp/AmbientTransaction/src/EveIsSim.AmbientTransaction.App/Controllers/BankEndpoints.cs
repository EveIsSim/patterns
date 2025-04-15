using System.Transactions;
using EveIsSim.AmbientTransaction.App.Controllers.Models;
using EveIsSim.AmbientTransaction.App.Services;

namespace EveIsSim.AmbientTransaction.App.Controllers;

public static class BankEndpoints
{
    public static IEndpointRouteBuilder MapBankEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/bank/transfer", async (IBankingService service, TransferRequest r, CancellationToken token) =>
                {
                    Console.WriteLine($"[Endpoint] Start: Thread: {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine($"[Endpoint] Start: Transaction.Current: {Transaction.Current?.TransactionInformation.LocalIdentifier}");

                    var result = await service.Transfer(r.ToDto(), token);

                    Console.WriteLine($"[Endpoint] Stop: After [TxFactory].Dispose: Thread: {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine($"[Endpoint] Stop: After [TxFactory].Dispose: Transaction.Current: {Transaction.Current?.TransactionInformation.LocalIdentifier}");

                    return result.IsSuccess
                        ? Results.Ok(new TransferReponse { })
                        : Results.BadRequest(new { ErrorMsg = result.ErrorMsg });
                });
        return builder;
    }
}
