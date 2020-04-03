using System;
using System.Collections.Generic;
using System.Text;

namespace HomeBudgetWf.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public DateTime DateOfregistration { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string OthersDetails { get; set; }


        public int ExpenseCategoryId { get; set; }
        public ExpenseCategory ExpenseCategory { get; set; }


    }
}
