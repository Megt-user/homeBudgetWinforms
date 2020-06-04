using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeBudgetWf.Models
{
    public class ExpenseCategory
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }

        public ICollection<KeyWord>? KeyWords { get; set; }
    }
}
