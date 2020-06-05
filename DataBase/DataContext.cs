using HomeBudgetWf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace HomeBudgetWf.DataBase
{
    public class DataContext : DbContext
    {
        private IConfiguration _configuration;

        //TODO get connection string https://stackoverflow.com/a/43514938
        public DataContext(DbContextOptions<DataContext> option,IConfiguration configuration) : base(option)
        {
            _configuration = configuration;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));
        //}
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<KeyWord> KeyWords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.KeyWord)
                .WithMany(c => c.Transactions);

            modelBuilder.Entity<KeyWord>()
                .HasOne(c => c.ExpenseCategory)
                .WithMany(d => d.KeyWords)
                .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
