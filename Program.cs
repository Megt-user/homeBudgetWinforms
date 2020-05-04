using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HomeBudgetWf.Converters;
using Microsoft.Extensions.Configuration;
using Serilog;
using HomeBudgetWf.DataBase;
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
            catch (Exception)
            {

                throw;
            }
        }

        private static void ReadExcelFile()
        {
            var streamFile = @"C:\Transactions\TestData\Transactions_FellesSparDin.xlsx";

            var transactionJsonArray = new ExcelConverter().GetJsonArrayfromExcelfile(streamFile);
            var transactionList = JsonConverter.ConvetJsonArrayToListTransaction(transactionJsonArray);

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
            var classThatLogs = new TransactionServices(_Iconfiguration);
            classThatLogs.AddTestdata();

        }

    }
}
