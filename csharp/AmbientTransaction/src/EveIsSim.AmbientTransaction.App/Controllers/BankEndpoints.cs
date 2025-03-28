using EveIsSim.AmbientTransaction.App.Controllers.Models;
using EveIsSim.AmbientTransaction.App.Services;

namespace EveIsSim.AmbientTransaction.App.Controllers;

public static class BankEndpoints
{
    public static IEndpointRouteBuilder MapBankEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/bank/transfer", async (IBankingService service, TransferRequest r, CancellationToken token) =>
                {
                    var result = await service.Transfer(r.ToDto(), token);

                    return result.IsSuccess
                        ? Results.Ok(new TransferReponse { })
                        : Results.BadRequest(new { ErrorMsg = result.ErrorMsg });
                });
        return builder;
    }
}
