namespace EveIsSim.AmbientTransaction.App.Repositories.Models;

public class AccountEntity
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
}
