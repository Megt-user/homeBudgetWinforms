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
            excelTable = CreateExcelTable(wsSheet, tableName, propertyNames, _StartCell, endTableCellAdress);

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
        private static ExcelTable CreateExcelTable(ExcelWorksheet wsSheet, string tableName, IEnumerable<string> excelColumns, string startTableCellAddress, string endTableCellAddress, bool showTotal = false)
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
                    excelWorksheet.Cells[ExcelHelpers.AddRowAndColumnToCellAddress(tableStartAdress, i, j)].Value = jObjectProperyValue;
                    j++;
                }
            }

        }

        public static List<ExcelTable> CreateExcelMonthSummaryTableFromMovementsViewModel(ExcelWorksheet wsSheet, List<TransactionWithCategory> movementsModel,
            IEnumerable<string> categories, int sheetYear = 0, string sheetTableName = null, bool? justExtrations = null, string startCell = null)
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
                minYear = movementsModel.Min(mov => mov.DateOfTransaction.Year);
                maxYear = movementsModel.Max(mov => mov.DateOfTransaction.Year);
            }


            IEnumerable<string> newColumns = new[] { "Month", "Total" };
            categories = Helpers.GetExtractionCategories(movementsModel, justExtrations);
            categories = categories.OrderBy(c => c);

            // and the new columns to the category
            categories = newColumns.Concat(categories);
            var categoriesUpdated = categories as string[] ?? categories.ToArray();

            string startTableCell = startCell ?? _StartCell;

            // Create Excel table Header
            var endTableCellAddress = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, 12, categoriesUpdated.Count() - 1);
            var tableName = sheetTableName ?? "Tanble-";
            List<ExcelTable> excelTables = new List<ExcelTable>();
            for (int year = minYear; year <= maxYear; year++)
            {
                //give table Name
                tableName = string.Concat(tableName, year);

                //calculate Table sizes
                endTableCellAddress = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, 12, categoriesUpdated.Count() - 1);
                var excelTable = CreateExcelTable(wsSheet, tableName, categoriesUpdated, startTableCell, endTableCellAddress, true);

                // Set Excel table content
                for (int month = 1; month <= 12; month++)
                {
                    for (int column = 0; column < categoriesUpdated.Length; column++)
                    {
                        switch (categoriesUpdated[column])
                        {
                            case "Month":
                               
                                if (DateTimeFormatInfo.CurrentInfo != null)
                                {
                                    var monthName = string.Concat(DateTimeFormatInfo.CurrentInfo.GetMonthName(month));
                                    excelTable.WorkSheet.Cells[ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, month, column)].Value = monthName;
                                }

                                break;
                            //case "Total":
                            //    double totalCategory = ModelConverter.CategoriesMonthYearTotal(movementsModel, year, month, justExtrations);
                            //    excelTable.WorkSheet.Cells[ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, month, column)].Value = totalCategory;
                            //    break;
                            //default:
                            //    //Get summ for category
                            //    var tablecell = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, month, column);
                            //    double totalCategory1 = ModelOperation.GetTotalforCategory(movementsModel, categoriesUpdated[column], year, month, justExtrations);
                            //    excelTable.WorkSheet.Cells[tablecell].Style.Numberformat.Format = ExcelHelpers.SetFormatToCell("Amount");
                            //    //add value tu excel cell
                            //    wsSheet.Cells[tablecell].Value = totalCategory1;
                            //    //AddExcelCellValue(row, tableStartColumn, totalCategory1, wsSheet);
                            //    break;
                        }
                    }
                }
                startTableCell = ExcelHelpers.AddRowAndColumnToCellAddress(startTableCell, 12 + 5, 0);
                excelTable.WorkSheet.Cells[wsSheet.Dimension.Address].AutoFitColumns();
                excelTables.Add(excelTable);
            }
            return excelTables;
        }

    }
}
