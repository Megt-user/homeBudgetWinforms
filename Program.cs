using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HomeBudgetWf.Excel;
using HomeBudgetWf.Json;
using Microsoft.Extensions.Configuration;
using Serilog;
using HomeBudgetWf.Utilities;
using OfficeOpenXml;

namespace HomeBudgetWf
{
    static class Program
    {
        public static IConfiguration _Iconfiguration;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SetUpConfiguration();
            SetupStaticLogger();
            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Log.Information("start App");
                //RunApp();
                ReadExcelFile();
                Application.Run(new TransactionForm());
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Problem operating WIinForm application");
            }
        }

        private static void ReadExcelFile()
        {
            var streamFile = @"C:\Users\MatiasGonzalezTognon\Dropbox (Personal)\Mis documentos\Flia G&S\Bancos\SparebankenDin\Transactions-Felles 22-05-17 til 18-08-20.xlsx";
            var streamFile1 = @"C:\Users\MatiasGonzalezTognon\Dropbox (Personal)\Mis documentos\Flia G&S\Proyectos\HomeBudget API\TestData\Categories.xlsx";

            var transactionJsonArray = new ExcelConverter().GetJsonArrayfromExcelfile(streamFile);
            var CategoriesJsonArray = new ExcelConverter().GetJsonArrayfromExcelfile(streamFile1);
            var transactionList = JsonConverter.ConvetJsonArrayToListTransaction(transactionJsonArray);
            
            var keyWords = JsonConverter.ConvertJsonArrayToListKeyWords(CategoriesJsonArray);
            var transactionListToSave = Converter.GetTransactionListToSave(transactionList, keyWords);
            var noko = Converter.ConvertTrasactionToTransactionWithCategories(transactionListToSave);
            //TODO transactionLis To excel
        }

        private static void SetUpConfiguration()
        {

            _Iconfiguration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
        private static void SetupStaticLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_Iconfiguration)
                .CreateLogger();
        }
      
    }
}
