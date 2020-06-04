using HomeBudgetWf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeBudgetWf.DataBase
{
    public class DataContext : DbContext
    {
        //TODO get connection string https://stackoverflow.com/a/43514938
        public DataContext(DbContextOptions<DataContext> option,IConfiguration configuration) : base(option)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connString);
        }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<KeyWord> KeyWords { get; set; }
    }
}
