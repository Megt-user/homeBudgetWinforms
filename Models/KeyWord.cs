using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeBudgetWf.Models
{
    public class KeyWord
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }

        public ExpenseCategory ExpenseCategory { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

    }
}
