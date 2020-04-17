using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeBudgetWf.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HomeBudgetWf.DataBase
{
    public class TransactionServices
    {

        private DataContext _dataContext;
        public TransactionServices()
        {
            _dataContext = new DesignTimeDbContextFactory().CreateDbContext(new String[] { });
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
