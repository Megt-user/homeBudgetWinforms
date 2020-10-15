using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using HomeBudgetWf.Json;
using HomeBudgetWf.Models;
using HomeBudgetWf.Utilities;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace HomeBudgetWf.Excel
{
    public class ExcelUtilitites
    {

        public static string _StartCell = "A3";
        private static string _hedingCell = ExcelHelpers.AddRowAndColumnToCellAddress(_StartCell, -2, 1);
        private static string _titlecell = ExcelHelpers.AddRowAndColumnToCellAddress(_StartCell, -2, 2);

        public ExcelTable CreateExcelTableFromMovementsViewModel(List<TransactionWithCategory> movementsModel, ExcelWorksheet wsSheet, string tableName)
        {
            //Get the list of Column that want to be created in the table
            var excelColumns = new TransactionWithCategory();
            var properties = excelColumns.GetType().GetProperties();
            var propertyNames = properties.Select(p => p.Name).ToList();


            ExcelTable excelTable = null;

            // Calculate size of the table
            var endTableCellAdress = ExcelHelpers.AddRowAndColumnToCellAddress(_StartCell, movementsModel.Count, propertyNames.Count - 1);

            // Create Excel table Header
            excelTable = CreateExcelTable(ref wsSheet, tableName, propertyNames, _StartCell, endTableCellAdress);

            for (int row = 0; row < movementsModel.Count; row++)
            {
                for (int column = 0; column < propertyNames.Count; column++)
                {
                    //Get Property name value
                    var propertyValue = Helpers.GetPropertyValue(movementsModel[row], propertyNames[column]);
                    string tableCellAdress = ExcelHelpers.AddRowAndColumnToCellAddress(_StartCell, row + 1, column);
                    excelTable.WorkSheet.Cells[tableCellAdress].Value = propertyValue;

                }
            }
            excelTable.WorkSheet.Cells[wsSheet.Dimension.Address].AutoFitColumns();
            return excelTable;
        }
        private static ExcelTable CreateExcelTable(ref ExcelWorksheet wsSheet, string tableName, IEnumerable<string> excelColumns, string startTableCellAddress, string endTableCellAddress, bool showTotal = false)
        {
            ExcelTable table;
            using (ExcelRange rng = wsSheet.Cells[$"{startTableCellAddress}:{endTableCellAddress}"])
            {
                //Indirectly access ExcelTableCollection class
                table = wsSheet.Tables.Add(rng, tableName);
            }

            //var color = Color.FromArgb(250, 199, 111);

            //Set Columns position & name
            var i = 0;
            foreach (var property in excelColumns)
            {
                table.Columns[i].Name = string.Concat(property);
                //Add Subtotal cell to the end of the table
                if (i != 0)
                    table.Columns[i].TotalsRowFormula = $"SUBTOTAL(101,[{property}])"; // 101 average, 103 Count, 109 sum
                i++;
            }

            table.ShowHeader = true;
            table.ShowFilter = true;
            table.ShowTotal = showTotal;
            return table;
        }

        public static ExcelTable AddTableToWorkSheet(ref ExcelWorksheet excelWorksheet, JArray jsonArray, string tableName)
        {
            var columnsList = JsonConverter.GetListOfKeyFromJArray(jsonArray);
            var startTableCellAddress = ExcelHelpers.AddRowAndColumnToCellAddress(_StartCell, 4, 0);
            var endTableCellAddress = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCellAddress, jsonArray.Count, columnsList.Count - 1);
            ExcelTable table;
            using (ExcelRange rng = excelWorksheet.Cells[$"{startTableCellAddress}:{endTableCellAddress}"])
            {
                table = excelWorksheet.Tables.Add(rng, tableName);
            }
            table.ShowHeader = true;
            table.ShowFilter = true;
            return table;
        }

        public static void AddHeadersToExcelTable(ExcelTable table, JArray jsonArray, bool? addSubtotal = null)
        {
            var columnsList = JsonConverter.GetListOfKeyFromJArray(jsonArray);

            //Set Columns position & name
            var i = 0;
            foreach (var property in columnsList)
            {
                table.Columns[i].Name = string.Concat(property);

                if (addSubtotal.HasValue)
                {
                    //TODO skip columns with text/date
                    table.Columns[i].TotalsRowFormula = $"SUBTOTAL(101,[{property}])"; // 101 average, 103 Count, 109 sum 
                }
                i++;
            }

        }
        public static void AddDataToTabel(ref ExcelWorksheet excelWorksheet, ExcelTable excelTable, JArray jsonArray)
        {
            var columnsList = JsonConverter.GetListOfKeyFromJArray(jsonArray);
            var tableStartAdress = excelTable.Address.Start.Address;
            var i = 0;
            foreach (var jsonObject in jsonArray.Children<JObject>())
            {
                i++;
                var j = 0;
                foreach (var columnName in columnsList)
                {
                    var jObjectProperyValue = jsonObject[columnName].ToString();
                    var tablecell = ExcelHelpers.AddRowAndColumnToCellAddress(tableStartAdress, i, j);
                    excelTable.WorkSheet.Cells[tablecell].Style.Numberformat.Format = SetFormatToCell(columnName);

                    if (columnName.Contains("date", StringComparison.CurrentCultureIgnoreCase))
                    {
                        excelWorksheet.Cells[tablecell].Formula = @"=DATEVALUE(""" + jObjectProperyValue + @""")";
                    }
                    else
                    {
                        double number;
                        if (double.TryParse(jObjectProperyValue, out number))
                        {
                            excelWorksheet.Cells[tablecell].Value = number;
                        }
                        else
                        {
                            excelWorksheet.Cells[tablecell].Value = jObjectProperyValue;
                        }
                    }

                    j++;
                }
            }

        }

        public static List<ExcelTable> CreateExcelMonthSummaryTableFromMovementsViewModel(ref ExcelWorksheet wsSheet, List<TransactionWithCategory> transactionWithCategories, int sheetYear = 0, string sheetTableName = null, bool? justExtrations = null, string startCell = null)
        {

            int minYear;
            int maxYear;
            if (sheetYear > 0)
            {
                minYear = sheetYear;
                maxYear = sheetYear;
            }
            else
            {
                minYear = transactionWithCategories.Min(mov => mov.DateOfTransaction.Year);
                maxYear = transactionWithCategories.Max(mov => mov.DateOfTransaction.Year);
            }


            IEnumerable<string> newColumns = new[] { "Month", "Total" };
            IEnumerable<string> categories = Helpers.GetCategoriesFromTransactions(transactionWithCategories, justExtrations);
            categories = categories.OrderBy(c => c);

            // Add the new columns to the category
            categories = newColumns.Concat(categories);
            var categoriesUpdated = categories as string[] ?? categories.ToArray();

            string startTableCell = startCell ?? _StartCell;

            // Create Excel table Header
            var endTableCellAddress = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, 12, categoriesUpdated.Count() - 1);
            var tableName = sheetTableName ?? "Tanble";
            List<ExcelTable> excelTables = new List<ExcelTable>();
            for (int year = minYear; year <= maxYear; year++)
            {
                //give table Name
                var newTableName = string.Concat(tableName, year);

                //Add name to table
                var tableHederCell = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, -1, 0);
                wsSheet.Cells[tableHederCell].Value = $"Table from {year} - Extraction:{justExtrations} ";

                //calculate Table sizes
                endTableCellAddress = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, 12, categoriesUpdated.Count() - 1);
                var excelTable = CreateExcelTable(ref wsSheet, newTableName, categoriesUpdated, startTableCell, endTableCellAddress, true);

                // Set Excel table content
                for (int month = 1; month <= 12; month++)
                {
                    for (int column = 0; column < categoriesUpdated.Length; column++)
                    {
                        var ColumnName = categoriesUpdated[column];
                        switch (ColumnName)
                        {
                            case "Month":

                                if (DateTimeFormatInfo.CurrentInfo != null)
                                {
                                    var monthName = string.Concat(DateTimeFormatInfo.CurrentInfo.GetMonthName(month));
                                    excelTable.WorkSheet.Cells[ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, month, column)].Value = monthName;
                                }

                                break;
                            case "Total":
                                var monthYearTransaction = Helpers.GetMonthYearTransaction(transactionWithCategories, year, month, justExtrations);
                                decimal totalCategory = CalculationUtilities.GetTotalFromTransactionsWithCategoryModel(monthYearTransaction, justExtrations);

                                //Add format and value to excel cell
                                var totalTablecell = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, month, column);
                                excelTable.WorkSheet.Cells[totalTablecell].Style.Numberformat.Format = SetFormatToCell("Amount");
                                excelTable.WorkSheet.Cells[totalTablecell].Value = totalCategory;

                                break;
                            default:

                                //filter transactions to get the total
                                var monthYearCategoryTransaction = Helpers.GetMonthYearTransaction(transactionWithCategories, year, month, justExtrations, ColumnName);
                                decimal totalMonthYearCategory = CalculationUtilities.GetTotalFromTransactionsWithCategoryModel(monthYearCategoryTransaction, justExtrations);


                                //Add format and value to excel cell
                                var tablecell = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, month, column);
                                excelTable.WorkSheet.Cells[tablecell].Style.Numberformat.Format = SetFormatToCell("Amount");
                                wsSheet.Cells[tablecell].Value = totalMonthYearCategory;

                                break;
                        }
                    }
                }
                startTableCell = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, 12 + 5, 0);
                excelTable.WorkSheet.Cells[wsSheet.Dimension.Address].AutoFitColumns();
                excelTables.Add(excelTable);
            }
            return excelTables;
        }

        internal static string SetFormatToCell(string value)
        {
            if (value.Contains("date", StringComparison.CurrentCultureIgnoreCase))
                value = "DateTime";


            switch (value)
            {
                case "Id":
                    return "#";
                case "DateTime":
                    return "dd/mm/yyyy";
                case "Amount":
                case "Balance":
                case "Decimal":
                    return "$ # ##0.00;[Red]$ -# ##0.00";
                default:
                    return "@";
            }
        }
    }
}
