using System;
using System.Collections.Generic;
using System.Text;
using HomeBudgetWf.Excel;
using HomeBudgetWf.Json;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.DataTable
{
    public static class DataTableConv
    {
        public static System.Data.DataTable toDataTable(JArray jArray)
        {
            var result = new System.Data.DataTable();
            //Initialize the columns, If you know the row type, replace this   
            foreach (var row in jArray)
            {
                foreach (var jToken in row)
                {
                    var jproperty = jToken as JProperty;
                    if (jproperty == null) continue;
                    if (result.Columns[jproperty.Name] == null)
                        result.Columns.Add(jproperty.Name, SetFormatToColumn(jproperty.Name));
                }
            }
            foreach (var row in jArray)
            {
                var datarow = result.NewRow();
                foreach (var jToken in row)
                {
                    var jProperty = jToken as JProperty;
                    if (jProperty == null) continue;
                    datarow[jProperty.Name] = jProperty.Value.ToString();
                }
                result.Rows.Add(datarow);
            }
            return result;
        }


        internal static Type SetFormatToColumn(string value)
        {
            if (value.Contains("date", StringComparison.CurrentCultureIgnoreCase))
                value = "DateTime";


            switch (value)
            {
                case "Id":
                    return typeof(int);
                case "DateTime":
                    return typeof(DateTime);
                case "Amount":
                case "Balance":
                case "Decimal":
                    return typeof(decimal);
                default:
                    return typeof(string);
            }
        }
    }

}
