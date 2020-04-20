using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeBudgetWf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace HomeBudgetWf.DataBase
{
    public class TransactionServices
    {

        private DataContext _dataContext;
        private readonly IConfiguration _configuration;

        public TransactionServices(IConfiguration configuration)
        {
            _configuration = configuration;
            Log.Information("the DB ConnectionString Is:{ConnectionString}", _configuration.GetConnectionString("default"));
            _dataContext = new DesignTimeDbContextFactory(configuration).CreateDbContext(new String[] { });
        }

        public void AddNewData(Transaction transaction)
        {
            var transactionExist = GetTransaction(transaction.Description);
            _dataContext.SaveChanges();
        }



        private Transaction GetTransaction(string description)
        {
            if (string.IsNullOrEmpty(description))
                return null;

            var transaction = _dataContext.Transactions
                .Include(Word => Word.KeyWord)
                .Include(cat => cat.KeyWord.ExpenseCategory)
                .SingleOrDefault(t => t.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase));
            return transaction;
        }


        public  void AddTestdata()
        {
            var structuredData = new StructuredData();
            var simpleData = "This is a string.";

            // Use the static Serilog.Log class for logging.
            Log.Verbose("Here's a Verbose message.");
            Log.Debug("Here's a Debug message. Only Public Properties (not fields) are shown on structured data. Structured data: {@sampleData}. Simple data: {simpleData}.", structuredData, simpleData);
            Log.Information(new Exception("Exceptions can be put on all log levels"), "Here's an Info message.");
            Log.Warning("Here's a Warning message.");
            Log.Error(new Exception("This is an exception."), "Here's an Error message.");
            Log.Fatal("Here's a Fatal message.");

            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 17),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("136.58"),
                Balance = decimal.Parse("136.58"),
                Description = "Rema 1000",
                OthersDetails = "other....",
                KeyWord = new KeyWord()
                {
                    Value = "Rema",
                    ExpenseCategory = new ExpenseCategory()
                    {
                        Category = "Super"
                    }
                }

            });
            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 17),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("136.58"),
                Balance = decimal.Parse("136.58"),
                Description = "Escuela SFO",
                OthersDetails = "other....",
                KeyWord = new KeyWord()
                {
                    Value = "Escuela",
                    ExpenseCategory = new ExpenseCategory()
                    {
                        Category = "Chicos"
                    }
                }

            }); _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 17),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("136.58"),
                Balance = decimal.Parse("136.58"),
                Description = "Gulbring cine",
                OthersDetails = "other....",
                KeyWord = new KeyWord()
                {
                    Value = "Cine",
                    ExpenseCategory = new ExpenseCategory()
                    {
                        Category = "Free time"
                    }
                }

            });
            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 17),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("136.58"),
                Balance = decimal.Parse("136.58"),
                Description = "Meni supermercado",
                OthersDetails = "other....",
                KeyWord = new KeyWord()
                {
                    Value = "Meni",
                    ExpenseCategory = new ExpenseCategory()
                    {
                        Category = "Super"
                    }
                }

            });
            _dataContext.SaveChanges();
        }
    }
}
