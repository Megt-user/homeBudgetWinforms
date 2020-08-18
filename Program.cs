using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HomeBudgetWf.Converters;
using Microsoft.Extensions.Configuration;
using Serilog;
using HomeBudgetWf.DataBase;
using HomeBudgetWf.Utilities;
using OfficeOpenXml;

namespace HomeBudgetWf
{
    static class Program
    {
        public static IConfiguration _Iconfiguration;

        public static TransactionServices _TransactionServices;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            SetUpConfiguration();
            SetupStaticLogger();
            SetUpDbConnection();
            try
            {
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Log.Information("start App");
                RunApp();
                //ReadExcelFile();
                Application.Run(new TransactionForm());
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Problem operating WIinForm application");
            }
        }

        private static void SetUpDbConnection()
        {
            try
            {
                _TransactionServices = new TransactionServices(_Iconfiguration);
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Problem Creating DB conection");
            }
        }

        private static void ReadExcelFile()
        {
            var streamFile = @"C:\Transactions\TestData\Transactions_FellesSparDin.xlsx";

            var transactionJsonArray = new ExcelConverter().GetJsonArrayfromExcelfile(streamFile);
            var transactionList = JsonConverter.ConvetJsonArrayToListTransaction(transactionJsonArray);
            var keyWords = _TransactionServices.GetKeyWords();
            var transactionListToSave = Converter.GetTransactionListToSave(transactionList, keyWords);
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
        private static void RunApp()
        {
            // Do not pass any logger in via Dependency Injection, as the class will simply reference the static logger.
            _TransactionServices.AddTestdata();

        }

    }
}
