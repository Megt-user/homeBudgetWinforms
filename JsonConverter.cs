using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeBudgetWf.Models;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf
{
    public class JsonConverter
    {
        private IEnumerable<object> jArray;

        public static List<Transaction> ConvertJArrayToTransactionList(JArray jsonArray)
        {
            return jsonArray.Select(item => (Transaction)ParseObjectProperties(new Transaction(), item)).ToList();
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
