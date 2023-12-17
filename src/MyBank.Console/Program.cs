using MyBank.Domain;

var bankAccount = new BankAccount("Enio", 1000);

Console.WriteLine("Account number {0}", bankAccount.Number);

try
{
    bankAccount.Deposit(4000, "Salary");
}
catch (ArgumentOutOfRangeException e)
{
    Console.WriteLine(e.Message);
}


Console.WriteLine("Balance {0}", bankAccount.Balance);

bankAccount.Withdrawal(1000, "Rent payment");

Console.WriteLine("Balance {0}", bankAccount.Balance);

bankAccount.Withdrawal(1350, "Rent payment");

Console.WriteLine("Balance {0}", bankAccount.Balance);

bankAccount.Transactions.ToList().ForEach(Console.WriteLine);

Console.WriteLine("Enter something:");
var text = Console.ReadLine();

Console.WriteLine("Value inputed: {0}", text);

Console.ReadKey();

