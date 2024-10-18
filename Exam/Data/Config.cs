using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public static class Config
    {
        public static IConfigurationRoot Configuration { get; set; }
        static Config()
        {
            //var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            //var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
            //                    devEnvironmentVariable.ToLower() == "development";

            var builder = new ConfigurationBuilder();
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //if (isDevelopment)
            //{
            //    builder.AddUserSecrets<App>();
            //}

            Configuration = builder.Build();
        }

        public static SQLiteDbContext DbContext => new SQLiteDbContext();
    }
}
