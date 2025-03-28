using EveIsSim.AmbientTransaction.App.Repositories.Models;

namespace EveIsSim.AmbientTransaction.App.Services.Models;

public class MoneyTransferDto
{
    public int SourceAccountId { get; set; }
    public int DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public bool IsFailTransaction { get; set; }


    public TransactionLogEntity ToLog()
    {
        return new TransactionLogEntity
        {
            SourceAccountId = SourceAccountId,
            DestinationAccountId = DestinationAccountId,
            Amount = Amount,
            Timestamp = DateTime.UtcNow
        };
    }
}
