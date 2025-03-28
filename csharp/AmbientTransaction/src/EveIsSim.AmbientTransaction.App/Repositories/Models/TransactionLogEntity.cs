namespace EveIsSim.AmbientTransaction.App.Repositories.Models;

public class TransactionLogEntity
{
    public int Id { get; set; }
    public int SourceAccountId { get; set; }
    public int DestinationAccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Timestamp { get; set; }
}
