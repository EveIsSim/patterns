using EveIsSim.AmbientTransaction.App.Services.Models;

namespace EveIsSim.AmbientTransaction.App.Controllers.Models;

public class TransferRequest
{
    public int SourceAccountId { get; set; }
    public int DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public bool IsFailTransaction { get; set; }


    public MoneyTransferDto ToDto()
    {
        return new MoneyTransferDto
        {
            SourceAccountId = SourceAccountId,
            DestinationAccountId = DestinationAccountId,
            Amount = Amount,
            IsFailTransaction = IsFailTransaction
        };
    }
}
