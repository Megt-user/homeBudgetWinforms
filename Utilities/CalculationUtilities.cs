using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeBudgetWf.Models;

namespace HomeBudgetWf.Utilities
{
    public class CalculationUtilities
    {
        public static decimal GetTotalFromTransactionsWithCategoryModel(List<TransactionWithCategory> transactionWithCategories, bool? extraction = null, string category = null)
        {
            if (transactionWithCategories == null || !transactionWithCategories.Any())
                return 0;

            decimal sum = 0;

            List<TransactionWithCategory> transactionToCalculate = category != null ? transactionWithCategories.Where(t => t.Category == category).ToList() : transactionWithCategories;

            if (extraction.HasValue)
            {
                if (extraction.GetValueOrDefault(false))
                {
                    var sum1 = transactionToCalculate.Where(mv => mv.Amount < 0).ToList();
                    sum = transactionToCalculate.Where(mv => mv.Amount < 0).Sum(cat => cat.Amount);
                }
                else
                {
                    var sum1 = transactionToCalculate.Where(mv => mv.Amount > 0).ToList();
                    sum = transactionToCalculate.Where(mv => mv.Amount > 0).Sum(cat => cat.Amount);
                }
            }
            else
            {
                sum = transactionToCalculate.Sum(cat => cat.Amount);
            }

            //return Math.Abs(sum);
            return sum;
        }
    }
}
