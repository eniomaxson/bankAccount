namespace MyBank.Domain;

public class BankAccount
{
    private List<Transaction> _transactions = new List<Transaction>();
    public string Number { get; }
    public string Owner { get; } // TODO: would be great if we have a class to represent a owner
    public decimal Balance => _transactions.Sum(t => t.Amount);

    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

    public BankAccount(string owner, decimal balance = 0)
    {
        if (balance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(balance));
        }

        Owner = owner;
        Number = GenerateAccountNumber();

        if (balance > 0)
        {
            _transactions.Add(new Transaction(TransactionsType.Deposit, balance, "Account creation"));
        }
    }

    /// <summary>
    /// Method responsible for deposit money on the bankAccount
    /// </summary>
    /// <param name="amount">Amount of money</param>
    /// <param name="note">Optional notes</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Deposit(decimal amount, string note = "")
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
        }

        var transaction = new Transaction(TransactionsType.Deposit, amount, note);
        _transactions.Add(transaction);
    }

    /// <summary>
    /// Method responsible for withdrawl money 
    /// </summary>
    /// <param name="amount">Amount of money</param>
    /// <param name="note">Optional notes</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Withdrawal(decimal amount, string note = "")
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
        }

        if (Balance < amount)
        {
            throw new InvalidOperationException("Not sufficient funds for this withdrawal");
        }

        var transaction = new Transaction(TransactionsType.Withdrawal, -amount, note);
        _transactions.Add(transaction);
    }

    /// <summary>
    /// Method responsible to transfer funds from one account to another
    /// </summary>
    /// <param name="from">The account where the money is leaving</param>
    /// <param name="to">The account where the money get into</param>
    /// <param name="amount">The value which will be transfered</param>
    /// <param name="note">Optional notes about the transaction</param>
    public void Transfer(BankAccount from, BankAccount to, decimal amount, string note = "")
    {
        // TODO: Implement this method 
        throw new NotImplementedException();
    }

    private string GenerateAccountNumber()
    {
        Random random = new();

        //var numbers = Enumerable.Range(0, 5)
        //                      .Select(_ => random.Next(0, 9))
        //                      .Select(n => n.ToString());

        var numbers = new string[5];

        foreach (var indice in Enumerable.Range(0, 5))
        {
            numbers[indice] = random.Next(0, 9).ToString();
        }

        return string.Join("", numbers);
    }
}
