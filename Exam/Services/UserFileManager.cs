using Exam.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Services
{
    public static class UserFileManager
    {
        public static string GetUserFilesPath(string postfix)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Config.Configuration["UserFolder"], postfix);
        }

        public static void WriteToUserFiles(string postfix, string data)
        {
            File.WriteAllText(GetUserFilesPath(postfix), data);
        }

        public static string ReadFromUserFiles(string postfix)
        {
            return File.ReadAllText(GetUserFilesPath(postfix));
        }
    }
}
