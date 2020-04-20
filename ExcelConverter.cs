using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HomeBudgetWf.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Serilog;


namespace HomeBudgetWf
{
    public class ExcelConverter
    {
        public JArray GetJsonArrayfromExcelfile(string path)
        {
            JArray jsonArray = null;
            FileInfo fi1 = new FileInfo(path);
            if (fi1.Exists)
            {
                using (var cashflowExcelPkg = new ExcelPackage(fi1))
                {
                    var expensesWSheet = cashflowExcelPkg.Workbook.Worksheets.FirstOrDefault();
                    if (expensesWSheet != null)
                    {
                        var transactions = expensesWSheet.Tables.FirstOrDefault();
                        jsonArray = ExcelConverter.GetJsonFromTable(transactions);
                    }
                    Log.Information("there is {TransactionCounts}", jsonArray.Count);
                }
            }
            else
            {
                Log.Error("Can't Open File'");
            }

            return jsonArray;
        }
        public static JArray GetJsonFromTable(ExcelTable table)
        {

            var jsonArray = new JArray();
            var dictionaryList = new List<Dictionary<string, string>>();
            var json = string.Empty;
            if (table != null)
            {
                var tableStartAdress = table.Address.Start.Address;
                var totalRows = table.Address.Rows;
                var totalColumns = table.Columns.Count;

                for (int row = 0 + 1; row < totalRows; row++)
                {
                    var valuesDictionary = new Dictionary<string, string>();

                    for (int column = 0; column < totalColumns; column++)
                    {
                        var objectName = table.WorkSheet.Cells[ExcelHelpers.AddRowAndColumnToCellAddress(tableStartAdress, 0, column)].Value;

                        var objectValue = table.WorkSheet.Cells[ExcelHelpers.AddRowAndColumnToCellAddress(tableStartAdress, row, column)].Value;

                        valuesDictionary.Add(objectName.ToString(), objectValue?.ToString());
                    }
                    dictionaryList.Add(valuesDictionary);
                }
                jsonArray = JArray.Parse(JsonConvert.SerializeObject(dictionaryList.ToArray()));
                json = Newtonsoft.Json.JsonConvert.SerializeObject(dictionaryList);
            }
            return jsonArray;
        }
    }
}
