using System;
using System.Collections.Generic;
using System.Text;

namespace HomeBudgetWf.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public IList<Transaction> Transactions { get; set; }
    }
}
