using Microsoft.EntityFrameworkCore;
using MyBank.API.Domain;

namespace MyBank.API.Data
{
    public class MyBankDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<BankAccount> BankAccounts => Set<BankAccount>();

        public MyBankDbContext(DbContextOptions<MyBankDbContext> options) : base(options)
        { }

        public virtual async ValueTask<int> AddAndSaveChangesAsync<T>(T model) where T : class
        {
            await AddAsync(model);

            return await SaveChangesAsync();
        }
    }
}
