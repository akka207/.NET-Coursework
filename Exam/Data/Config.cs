using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Exam.Data
{
    public static class Config
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static PM.DbContext.BaseDbContext DbContext;
        static Config()
        {
            var devEnvironmentVariable = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) ||
                                devEnvironmentVariable.ToLower() == "development";

            var builder = new ConfigurationBuilder();
            builder
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (isDevelopment)
            {
                builder.AddUserSecrets<App>();
            }

            Configuration = builder.Build();

            switch(Configuration.GetSection("ConnectionType").Value)
            {
                case "PostgreSQL":
                    DbContext = new PM.PostgreSQL.PostgresDbContext();
                    break;
                case "SQLite":
                    DbContext = new PM.SQLite.SQLiteDbContext();
                    break;
            }
        }

    }
}
