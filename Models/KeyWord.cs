using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeBudgetWf.Models
{
    public class KeyWord
    {
       public string Value { get; set; }

        public ExpenseCategory ExpenseCategory { get; set; }
    }
}
