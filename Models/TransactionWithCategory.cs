using System;
using System.Collections.Generic;
using System.Text;

namespace HomeBudgetWf.Models
{
    public class TransactionWithCategory
    {
      
        public DateTime DateOfTransaction { get; set; }
        public DateTime DateOfregistration { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string? OthersDetails { get; set; }
        public string KeyWord { get; set; }
        public string Category { get; set; }

        public TransactionWithCategory GetTransactionWithCategory(Transaction transaction)
        {
            DateOfTransaction = transaction.DateOfTransaction;
            DateOfregistration = transaction.DateOfregistration;
            Description = transaction.Description;
            Amount = transaction.Amount;
            Balance = transaction.Balance;
            OthersDetails = transaction.OthersDetails;
            KeyWord = transaction.KeyWord?.Value;
            Category = transaction.KeyWord?.ExpenseCategory?.Category;
            return this;
        }
    }
}
