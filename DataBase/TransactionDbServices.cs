using System;
using System.Collections.Generic;
using System.Text;
using HomeBudgetWf.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace HomeBudgetWf.DataBase
{
    public class TransactionDbServices
    {
       

        public static void AddNewData()
        {
            using (var noko = new DesignTimeDbContextFactory().CreateDbContext(new String[] { }))
            {
                noko.Transactions.Add(new Transaction()
                {
                    DateOfTransaction = new DateTime(2020, 04, 17),
                    DateOfregistration = new DateTime(2020, 04, 17),
                    Amount = decimal.Parse("136.58"),
                    Balance = decimal.Parse("136.58"),
                    Description = "noko",
                    OthersDetails = "other...."
                });
                noko.SaveChanges();
            }

        }
    }
}
