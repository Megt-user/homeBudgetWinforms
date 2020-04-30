using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HomeBudgetWf.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.Models.BanksModel
{
    //class 
    //public class SparebankenDinTransactionClass : TransactionAbstractClass
    //{
    //    private DateTime _DateOfTransaction;
    //    private DateTime _DateOfregistration;
    //    private string _Description;
    //    private decimal _Amount;
    //    private decimal _Balance;
    //    private string _bankName;

    //    public SparebankenDinTransactionClass(JToken jToken)
    //    {

    //        foreach (PropertyInfo propertyInfo in typeof(TransactionAbstractClass).GetProperties())
    //        {
    //            var propertyName = propertyInfo.Name;
    //            var jsonPropertyValue = jToken[propertyName];
    //            if (jsonPropertyValue != null)
    //            {
    //                var jtokenValue = jsonPropertyValue.ToString();
    //                var propertyType = propertyInfo.PropertyType.Name;
    //                var value =JsonConverterClass.ParseObjectValue(propertyType, jtokenValue);

    //                if (propertyName.Contains("DateOfregistration"))
    //                {
    //                    _DateOfTransaction = (DateTime) value;
    //                }
    //                propertyInfo.SetValue(_DateOfTransaction, value);
    //            }
    //        }

    //    }
    //    public override string BankName
    //    {
    //        get { return _bankName; }
    //    }
    //    public override DateTime DateOfTransaction
    //    {
    //        get { return _DateOfTransaction; }
    //        set { _DateOfTransaction = value; }
    //    }
    //    public override DateTime DateOfregistration
    //    {
    //        get { return _DateOfregistration; }
    //        set { _DateOfregistration = value; }
    //    }
    //    public override string Description
    //    {
    //        get { return _Description; }
    //        set { _Description = value; }
    //    }
    //    public override decimal Amount
    //    {
    //        get { return _Amount; }
    //        set { _Amount = value; }
    //    }
    //    public override decimal Balance
    //    {
    //        get { return _Balance; }
    //        set { _Balance = value; }
    //    }
    //}
}
