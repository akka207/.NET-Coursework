using Microsoft.EntityFrameworkCore;
using StaffManagerModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Data
{
    public static class DBController
    {
        public static readonly IDBController Instance;

        static DBController()
        {
            switch (Config.Configuration.GetSection("ConnectionType").Value)
            {
                case "API":
                    Instance = new APIDBController();
                    break;
                case "LOCAL":
                    Instance = new LocalDBController();
                    break;
            }
        }
    }
}
