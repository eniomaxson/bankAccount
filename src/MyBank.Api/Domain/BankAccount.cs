using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBank.API.Domain
{
    [Table("BankAccounts")]
    public class BankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Owner { get; set; } = "";
        public string Number { get; set; } = "";
        public decimal Balance { get; set; }
        public List<BankTransaction> Transactions { get; set; } = new();
    }
}
