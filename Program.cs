using System;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Serilog;
using HomeBudgetWf.DataBase;

// https://stackoverflow.com/a/27509005

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
                RunApp();
                Application.Run(new TransactionForm());

            }
            catch (Exception)
            {

                throw;
            }
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
                //.WriteTo.Console()
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
