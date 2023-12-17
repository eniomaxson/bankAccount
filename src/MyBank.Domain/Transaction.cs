namespace MyBank.Domain;

public class Transaction
{
    public TransactionsType Type { get; }
    public decimal Amount { get; }
    public DateTime Date { get; }
    public string Notes { get; }

    public Transaction(TransactionsType type, decimal amount, string note)
    {
        Type = type;
        Amount = amount;
        Notes = note;

        Date = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{Date},{Type}: {Amount}";
    }
}
