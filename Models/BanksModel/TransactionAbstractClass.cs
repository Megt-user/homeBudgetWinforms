using System;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.Models.BanksModel
{
    public abstract class TransactionAbstractClass
    {
        public abstract DateTime DateOfTransaction { get; set; }
        public abstract DateTime DateOfregistration { get; set; }
        public abstract string Description { get; set; }
        public abstract decimal Amount { get; set; }
        public abstract decimal Balance { get; set; }
        public abstract string BankName { get; }
    }
}
