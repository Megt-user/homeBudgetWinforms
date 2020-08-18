using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HomeBudgetWf.DataBase
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            //https://stackoverflow.com/a/53924899

            // IDesignTimeDbContextFactory is used usually when you execute EF Core commands like Add-Migration, Update-Database, and so on
            // So it is usually your local development machine environment
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Prepare configuration builder
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json", optional: false)
                //.AddJsonFile($"appsettings.{envName}.json", optional: false)
                .Build();

            // Bind your custom settings class instance to values from appsettings.json
            //var settingsSection = configuration.GetSection("Settings");
            //var appSettings = new AppSettings();
            //settingsSection.Bind(appSettings);

            // Create DB context with connection from your AppSettings 
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"));


            return new DataContext(optionsBuilder.Options);
        }


    }
}
