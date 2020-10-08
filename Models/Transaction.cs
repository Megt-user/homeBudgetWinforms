using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeBudgetWf.Models
{
    public class Transaction
    {
        public Transaction()
        {
            
        } 
        public Transaction(Transaction transaction)
        {
            DateOfTransaction = transaction.DateOfTransaction;
            DateOfregistration = transaction.DateOfregistration;
            Description = transaction.Description;
            Amount = transaction.Amount;
            Balance = transaction.Balance;
            OthersDetails = transaction.OthersDetails;
            if (transaction.KeyWord != null)
            {
                KeyWord = transaction.KeyWord;
            }
        }
        [Key]
        public int Id { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public DateTime DateOfregistration { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        public string? OthersDetails { get; set; }

        public KeyWord? KeyWord { get; set; }

        //TODO Add Acount Info, bank, count number, etc.

    }
}
