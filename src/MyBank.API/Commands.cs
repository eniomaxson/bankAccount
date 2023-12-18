namespace MyBank.API
{
    public record OpenAccountCommand(string Owner, decimal initialDeposit = 0);

    public record DepositCommand(decimal Ammount, string Note = "");

    public record WithDrawlCommand(decimal Ammount, string Note = "");
}
