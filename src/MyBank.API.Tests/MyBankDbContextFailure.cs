using Microsoft.EntityFrameworkCore;
using MyBank.API.Data;

namespace MyBank.API.Tests
{
    public class MyBankDbContextFailure : MyBankDbContext
    {
        public MyBankDbContextFailure(DbContextOptions<MyBankDbContext> options) : base(options)
        { }

        public override ValueTask<int> AddAndSaveChangesAsync<T>(T model)
        {
            throw new Exception("Database exception");
        }
    }
}
