using System;
using System.Collections.Generic;
using System.Globalization;
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

            _dataContext = new DesignTimeDbContextFactory().CreateDbContext(new String[] { });
        }

        public void AddNewData(Transaction transaction)
        {
            var transactionExist = GetTransactionByDescription(transaction.Description);
            var transactions = GetTransactionByDateTime(transaction.DateOfTransaction);
            if (transactions != null && transactions.Any())
            {
                if (transactions.Any(tr => tr.Description.Equals(transaction.Description)))
                {
                    return;
                }
            }

            _dataContext.Transactions.Add(new Transaction(transaction));
            _dataContext.SaveChanges();
        }



        public Transaction[] GetTransactionByKeyWord(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return null;
            var key = GetKeyword(keyword);
            var transaction = _dataContext.Transactions
                .Include(cat => cat.KeyWord)
                .Include(cat => cat.KeyWord.ExpenseCategory)
                .Where(word => word.KeyWord.Id == key.Id).ToArray();
            return transaction;
        }
        public Transaction GetTransactionByDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return null;

            var transaction = _dataContext.Transactions
                .Include(word => word.KeyWord)
                .Include(cat => cat.KeyWord.ExpenseCategory)
                .SingleOrDefault(t => t.Description.Equals(description, StringComparison.InvariantCultureIgnoreCase));
            return transaction;
        }
        public IQueryable<Transaction> GetTransactionByDateTime(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(dateTime.ToString(CultureInfo.InvariantCulture)))
                return null;

            var transaction = _dataContext.Transactions
                .Include(word => word.KeyWord)
                .Include(cat => cat.KeyWord.ExpenseCategory)
                .Where(t => t.DateOfTransaction.Equals(dateTime));
            return transaction;
        }


        public void AddTestdata()
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

            List<ExpenseCategory> expenseCategories = new List<ExpenseCategory>()
            {
                new ExpenseCategory(){Category = "Super"},
                new ExpenseCategory(){Category = "Free time"},
                new ExpenseCategory(){Category = "Chicos"},
            };
            List<KeyWord> keyWords = new List<KeyWord>()
            {
                new KeyWord(){Value = "Rema",ExpenseCategory =expenseCategories[0]},
                new KeyWord(){Value = "Cine",ExpenseCategory =expenseCategories[1]},
                new KeyWord(){Value = "escuela",ExpenseCategory =expenseCategories[2]},
                new KeyWord(){Value = "Meni",ExpenseCategory =expenseCategories[0]},
                new KeyWord(){Value = "Kiwy",ExpenseCategory =expenseCategories[0]},
                new KeyWord(){Value = "Hovden",ExpenseCategory =expenseCategories[1]},
            };
            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 01),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("10.50"),
                Balance = decimal.Parse("100"),
                Description = "Noko Hovden 1000",
                OthersDetails = "other....",
                KeyWord = keyWords[5]

            });  
            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 01),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("10.50"),
                Balance = decimal.Parse("100"),
                Description = "Rema 1000",
                OthersDetails = "other....",
                KeyWord = keyWords[0]

            });
            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 03),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("10"),
                Balance = decimal.Parse("70"),
                Description = "Gulbring cine",
                OthersDetails = "other....",
                KeyWord = keyWords[1]


            });
            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 02),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("20"),
                Balance = decimal.Parse("80"),
                Description = "Escuela SFO",
                OthersDetails = "other....",
                KeyWord = keyWords[2]

            });

            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 04),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("15"),
                Balance = decimal.Parse("55"),
                Description = "Meni supermercado",
                OthersDetails = "other....",
                KeyWord = keyWords[3]

            });
            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 04),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("15"),
                Balance = decimal.Parse("55"),
                Description = "Kiwy supermercado",
                OthersDetails = "other....",
                KeyWord = keyWords[4]

            });
            _dataContext.Transactions.Add(new Transaction()
            {
                DateOfTransaction = new DateTime(2020, 04, 04),
                DateOfregistration = new DateTime(2020, 04, 17),
                Amount = decimal.Parse("5"),
                Balance = decimal.Parse("50"),
                Description = "Meni supermercado",
                OthersDetails = "other....",
                KeyWord = keyWords[3]

            });
            _dataContext.SaveChanges();
        }

        public KeyWord AddKeyword(string keyword, string categoryName)
        {
            var KeyWord = new KeyWord()
            {
                Value = "Meni",
            };
            var expenseCategory = new ExpenseCategory()
            {
                Category = "Super"
            };
            _dataContext.KeyWords.Add(KeyWord);

            return KeyWord;
        }
        public ExpenseCategory GetExpenseCategory(string keyword)
        {
            var keyWord = _dataContext.ExpenseCategories
                .First(ke => ke.Category == keyword);
            return keyWord ?? null;
        }

        public KeyWord GetKeyword(string keyword)
        {
            var keyWord = _dataContext.KeyWords
                .Include(sub => sub.ExpenseCategory)
                .First(ke => ke.Value == keyword);
            return keyWord ?? null;
        }


        public KeyWord[] GetKeyWords()
        {
            var keyWords = _dataContext.KeyWords
                .Include(sub => sub.ExpenseCategory);
            return keyWords.Any() ? keyWords.ToArray() : null;
        }
    }
}
