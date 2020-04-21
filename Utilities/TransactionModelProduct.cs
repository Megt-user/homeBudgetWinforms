using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HomeBudgetWf.Models;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.Utilities
{
   public abstract class TransactionModel
    {
        public abstract DateTime DateOfTransaction { get; set; }
        public abstract DateTime DateOfregistration { get; set; }
        public abstract string Description { get; set; }
        public abstract decimal Amount { get; set; }
        public abstract decimal Balance { get; set; }
        public abstract string BankName { get; }
    }
   public class TransactionModelSantander : TransactionModel
    {
        private DateTime _dateOfTransaction;
        private DateTime _dateOfregistration;
        private string _description;
        private decimal _amount;
        private decimal _balance;
        private string _bankName;
        public TransactionModelSantander(JToken jToken)
        {

            var transactionTemp =JsonConverter.ParseObjectProperties(new Transaction(), jToken) as Transaction;
            _dateOfTransaction = transactionTemp.DateOfTransaction;
            _dateOfregistration = transactionTemp.DateOfregistration;
            _description = transactionTemp.Description;
            _amount = transactionTemp.Amount;
            _balance = transactionTemp.Balance;
            _bankName = "Santander Rio";
        }

        public override string BankName
        {
            get { return _bankName; }
        }
        public override DateTime DateOfTransaction
        {
            get { return _dateOfTransaction; }
            set { _dateOfTransaction = value; }
        }
        public override DateTime DateOfregistration
        {
            get { return _dateOfregistration; }
            set { _dateOfregistration = value; }
        }
        public override string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public override decimal Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }
        public override decimal Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }
    }
   public class TransactionModelSparebankenDin : TransactionModel
    {
        private DateTime _DateOfTransaction;
        private DateTime _DateOfregistration;
        private string _Description;
        private decimal _Amount;
        private decimal _Balance;
        private string _bankName;

        public TransactionModelSparebankenDin(JArray jArray)
        {

        }
        public override string BankName
        {
            get { return _bankName; }
        }
        public override DateTime DateOfTransaction
        {
            get { return _DateOfTransaction; }
            set { _DateOfTransaction = value; }
        }
        public override DateTime DateOfregistration
        {
            get { return _DateOfregistration; }
            set { _DateOfregistration = value; }
        }
        public override string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public override decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }
        public override decimal Balance
        {
            get { return _Balance; }
            set { _Balance = value; }
        }
    }
}
