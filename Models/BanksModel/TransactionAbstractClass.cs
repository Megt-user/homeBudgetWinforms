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

    public class SantanderRioTransactionClass : TransactionAbstractClass
    {
        private DateTime _dateOfTransaction;
        private DateTime _dateOfregistration;
        private string _description;
        private decimal _amount;
        private decimal _balance;
        private readonly string _bankName;
        public SantanderRioTransactionClass(JToken jToken)
        {

            _dateOfTransaction = jToken.Value<DateTime?>("Fecha") ?? new DateTime();
            _dateOfregistration = jToken.Value<DateTime?>("Fecha") ?? new DateTime();
            _description = jToken.Value<string>("Descripción") ?? "Now description found";
            _amount = jToken.Value<decimal?>("Importe") ?? 0; ;
            _balance = jToken.Value<decimal?>("Saldo") ?? 0;
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
    class SparebankenDinTransactionClass : TransactionAbstractClass
    {
        private DateTime _dateOfTransaction;
        private DateTime _dateOfregistration;
        private string _description;
        private decimal _amount;
        private decimal _balance;
        private readonly string _bankName;

        public SparebankenDinTransactionClass(JToken jToken)
        {

            _dateOfTransaction = jToken.Value<DateTime?>("DateOfTransaction") ?? new DateTime();
            _dateOfregistration = jToken.Value<DateTime?>("DateOfregistration") ?? new DateTime();
            _description = jToken.Value<string>("Description") ?? "Now description found";
            _amount = jToken.Value<decimal?>("Amount") ?? 0; ;
            _balance = jToken.Value<decimal?>("Balance") ?? 0;
            _bankName = "Sparebanken din";

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
}
