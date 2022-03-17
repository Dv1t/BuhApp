using Microsoft.EntityFrameworkCore;

namespace BuhServer.Models
{
    public class TransactionsDbContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}
