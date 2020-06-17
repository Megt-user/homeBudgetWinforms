using HomeBudgetWf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace HomeBudgetWf.DataBase
{
    public class DataContext : DbContext
    {
       
        public DataContext(DbContextOptions<DataContext> option) : base(option)
        {

        }
      
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<KeyWord> KeyWords { get; set; }

        
    }
}
