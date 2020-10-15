using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeBudgetWf.Excel
{
    public class ExcelHelpers
    {
        public static string AddRowAndColumnToCellAddress(string address, int row, int column)
        {
            var addressAndWorkSheet = address.Split("!");

            var cellAddress = addressAndWorkSheet.Length > 1 ? addressAndWorkSheet[1] : addressAndWorkSheet[0];

            var dictionaryKeyIndex = GetRowAndColumIndex(cellAddress);

            if (dictionaryKeyIndex.Any())
            {
                var newaddress = $"{GetColumnName(dictionaryKeyIndex["column"] + column)}{dictionaryKeyIndex["row"] + row}";
                return addressAndWorkSheet.Length > 1 ? $"{addressAndWorkSheet[0]}!{newaddress}" : newaddress;
            }
            return null;
        }
        public static Dictionary<string, int> GetRowAndColumIndex(string address)
        {

            if (!String.IsNullOrEmpty(address))
            {

                var addressAndWorkSheet = address.Split("!");

                var cellAddress = addressAndWorkSheet.Length > 1 ? addressAndWorkSheet[1] : addressAndWorkSheet[0];


                Dictionary<string, int> dictionay = new Dictionary<string, int>();

                var column = String.Empty;
                var row = String.Empty;

                foreach (char c in cellAddress)
                {
                    if (Char.IsLetter(c))
                        column += c;
                    if (Char.IsNumber(c))
                        row += c;
                }
                int rowNumber;
                Int32.TryParse(row, out rowNumber);

                dictionay.Add("row", rowNumber);
                dictionay.Add("column", GetColumnIndex(column));
                if (addressAndWorkSheet.Length > 1)
                {
                    dictionay.Add("WorkSheet", 0);
                }
                return dictionay;
            }
            return null;
        }
        public static int GetColumnIndex(string columnName)
        {
            var index = 0;
            for (int i = 0; i < columnName.Length; i++)
            {
                index *= 26;
                index += (columnName[i] - 'A' + 1);
            }

            return index;
        }
        public static string GetColumnName(int index)
        {
            int dividend = index;
            string columnName = String.Empty;

            while (dividend > 0)
            {
                var modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }
       
    }
}
