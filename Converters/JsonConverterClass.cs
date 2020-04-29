using System;
using System.Collections.Generic;
using System.Linq;
using HomeBudgetWf.Models;
using HomeBudgetWf.Models.BanksModel;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.Converters
{
    public class JsonConverterClass : IJsonConverter
    {
        private IEnumerable<object> jArray;

        //public List<TransactionAbstractClass> ConvertJArrayToTransactionList(JArray jsonArray)
        //{
        //    //return jsonArray.Select(item => ParseObjectProperties<SantanderRioTransactionClass>(item)).ToList();
        //}

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

        public static object ParseJtokenToTransactionAbstractCalss(Object model, JToken json)
        {

            var type = model.GetType();
            var typeProperties = type.GetProperties();

            //SantanderRioTransactionClass santanderRio= new SantanderRioTransactionClass(json);
            
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

        //public static T ParseJtokenToType<T>(JToken json)
        //{
        //    var type = typeof(T);

        //    TransactionAbstractClass transaction;
        //    //TODO createInstance of Abstract Class
        //    type.GetMethods().Initialize();
        //    //var model = Activator.CreateInstance(type);
        //    var typeProperties = type.GetProperties();
        //    Object model;
        //    foreach (var property in typeProperties)
        //    {
        //        var propertyName = property.Name;
        //        var jsonPropertyValue = json[propertyName];
        //        if (jsonPropertyValue != null)
        //        {
        //            var jtokenValue = jsonPropertyValue.ToString();
        //            var propertyType = property.PropertyType.Name;
        //            var value = ParseObjectValue(propertyType, jtokenValue);
        //            property.SetValue(model, value);
        //        }
        //    }
        //    return (T) model;
        //}
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
