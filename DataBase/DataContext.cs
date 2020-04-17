using System.Configuration;
using HomeBudgetWf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HomeBudgetWf.DataBase
{
    public class DataContext : DbContext
    {
        //private readonly string _connectionString;
        //public DataContext(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("Default");

        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<KeyWord> KeyWords { get; set; }
    }
}
