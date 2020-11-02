using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using HomeBudgetWf.Excel;
using HomeBudgetWf.Json;
using HomeBudgetWf.Models;
using HomeBudgetWf.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;

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

                    var jpropertyName = jproperty.Name;
                    if (result.Columns[jpropertyName] == null)
                        result.Columns.Add(jpropertyName, SetFormatToColumn(jpropertyName));
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

        public static System.Data.DataTable CreateTableMonthSummaryFromMovementsViewModel(List<TransactionWithCategory> transactionWithCategories, int year = 0, bool? justExtrations = null)
        {

            IEnumerable<string> newColumns = new[] { "Month", "Total" };
            IEnumerable<string> categories = Helpers.GetCategoriesFromTransactions(transactionWithCategories, justExtrations);
            categories = categories.OrderBy(c => c);
            // Add the new columns to the category
            categories = newColumns.Concat(categories);
            var categoriesUpdated = categories as string[] ?? categories.ToArray();
            var result = new System.Data.DataTable();

            foreach (var category in categoriesUpdated)
            {
                if (!string.IsNullOrEmpty(category))
                {
                    result.Columns.Add(category, SetFormatToColumn(category));
                }
            }

            for (int month = 1; month <= 12; month++)
            {
                var datarow = result.NewRow();

                for (int column = 0; column < categoriesUpdated.Length; column++)
                {
                    var ColumnName = categoriesUpdated[column];
                    switch (ColumnName)
                    {
                        case "Month":

                            if (DateTimeFormatInfo.CurrentInfo != null)
                            {
                                var monthName = string.Concat(DateTimeFormatInfo.CurrentInfo.GetMonthName(month));
                                datarow["Month"] = monthName;
                            }

                            break;
                        case "Total":
                            var monthYearTransaction = Helpers.GetMonthYearTransaction(transactionWithCategories, year, month, justExtrations);
                            decimal totalCategory = CalculationUtilities.GetTotalFromTransactionsWithCategoryModel(monthYearTransaction, justExtrations);
                            datarow["Total"] = totalCategory;


                            break;
                        default:
                            if (!string.IsNullOrEmpty(ColumnName))
                            {
                                //filter transactions to get the total
                                var monthYearCategoryTransaction = Helpers.GetMonthYearTransaction(transactionWithCategories, year, month, justExtrations, ColumnName);
                                decimal totalMonthYearCategory = CalculationUtilities.GetTotalFromTransactionsWithCategoryModel(monthYearCategoryTransaction, justExtrations);
                                datarow[ColumnName] = totalMonthYearCategory;
                            }
                            break;
                    }
                }
                result.Rows.Add(datarow);
            }

            return result;
        }
        internal static Type SetFormatToColumn(string jpropertyName)
        {
            if (jpropertyName.Contains("date", StringComparison.CurrentCultureIgnoreCase))
                jpropertyName = "DateTime";

            var lowercasePropertyName = jpropertyName.ToLower();
            switch (jpropertyName)
            {
                case "id":
                    return typeof(int);
                case "datetime":
                    return typeof(DateTime);
                case "amount":
                case "balance":
                case "decimal":
                case "total":
                    return typeof(decimal);
                default:
                    return typeof(string);
            }
        }
    }

}
