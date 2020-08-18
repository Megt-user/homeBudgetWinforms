using System;
using System.Collections.Generic;
using System.Linq;
using HomeBudgetWf.Models;
using HomeBudgetWf.Models.BanksModel;
using HomeBudgetWf.Utilities;
using Newtonsoft.Json.Linq;
using Serilog;

namespace HomeBudgetWf.Converters
{


    public class JsonConverter
    {
        private static List<ExpenseCategory> _expenseCategories;
        public static TransactionAbstractClass[] ConvetJsonArrayToListTransaction(JArray jsonArray)
        {
            var transactions = new List<TransactionAbstractClass>();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                TransactionClassFactory transactionFactory = null;
                try
                {
                    transactionFactory = GetTransactionClassFromJtoken(jsonArray[i]);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error Geting Transaction from Jtoken index/row {jArrayIndex}", i);
                }

                try
                {
                    TransactionAbstractClass transactionAbstract = transactionFactory?.CreateTransactionClass();
                    if (transactionAbstract != null)
                    {
                        transactions.Add(transactionAbstract);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error can't create Transaction abstarct class from Jtoken index/row {jArrayIndex}", i);
                }
            }
            return transactions.ToArray();
        }


        public static KeyWord[] ConvertJsonArrayToListKeyWords(JArray jsonArray)
        {
            var keyWords = new List<KeyWord>();
            for (int i = 0; i < jsonArray.Count; i++)
            {
                KeyWord keyWord = null;
                try
                {
                    keyWord = GetTransactionClassFromJtoken(jsonArray[i]);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error Geting Transaction from Jtoken index/row {jArrayIndex}", i);
                }

                try
                {
                    TransactionAbstractClass transactionAbstract = transactionFactory?.CreateTransactionClass();
                    if (transactionAbstract != null)
                    {
                        transactions.Add(transactionAbstract);
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e, "Error can't create Transaction abstarct class from Jtoken index/row {jArrayIndex}", i);
                }
            }

            return keyWords.ToArray();
        }

        public static KeyWord GetKeyWord(JToken jToken)
        {
            KeyWord keyWord = new KeyWord();
            _expenseCategories = _expenseCategories ?? new List<ExpenseCategory>();
            var keyWordValue = jToken.Value<string>("Value");
            var expenseCategory = jToken.Value<string>("ExpenseCategory");
            if (_expenseCategories.Any(exp=>exp.Category.Equals(expenseCategory,StringComparison.InvariantCultureIgnoreCase)))
            {
                
            }



            return keyWord;
        }
        public static TransactionClassFactory GetTransactionClassFromJtoken(JToken jToken)
        {
            var banckName = Helpers.GetBanckName(jToken);
            TransactionClassFactory transactionFactory = null;
            switch (banckName)
            {
                case "Santander Rio":
                    transactionFactory = new SantanderRioFactory(jToken);
                    break;
                default:
                    transactionFactory = new SparebankenDinFactory(jToken);
                    break;
            }
            return transactionFactory;
        }

        public static object ParseObjectProperties(Object model, JToken json)
        {

            var type = model.GetType();
            var typeProperties = type.GetProperties();

            foreach (var property in typeProperties)
            {
                var propertyName = property.Name;
                var jsonPropertyValue = json[propertyName];
                if (jsonPropertyValue != null)
                {
                    var jtokenValue = jsonPropertyValue.ToString();
                    var propertyType = property.PropertyType.Name;
                    var value = ParseObjectValue(propertyType, jtokenValue);
                    property.SetValue(model, value);
                }
            }
            return model;
        }
        public static object ParseObjectValue(string type, string value)
        {
            switch (type.ToLower())
            {
                case "string":
                    return value;
                case "datetime":
                    DateTime dateTime;
                    if (DateTime.TryParse(value, out dateTime))
                        return dateTime;
                    return null;

                case "int16":
                case "int32":
                case "int64":
                case "integer":
                    int integer;
                    if (int.TryParse(value, out integer))
                        return integer;
                    return null;
                case "double":
                    double doubleValue;
                    if (double.TryParse(value, out doubleValue))
                        return doubleValue;
                    return null;
                case "decimal":
                    decimal decimalValue;
                    if (decimal.TryParse(value, out decimalValue))
                        return decimalValue;
                    return null;
                default:
                    return null;
            }
        }
    }
}
