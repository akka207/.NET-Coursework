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
        public static string GetPath(string postfix)
        {
            string? userFolder = Config.Configuration["UserFolder"];
            if (!string.IsNullOrWhiteSpace(userFolder))
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), userFolder, postfix);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void Write(string postfix, string data)
        {
            File.WriteAllText(GetPath(postfix), data);
        }

        public static string Read(string postfix)
        {
            string path = GetPath(postfix);
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            else
            {
                return string.Empty;
            }
        }

        public static void Clear(string postfix)
        {
            if(File.Exists(GetPath(postfix)))
                File.Delete(GetPath(postfix));
        }
    }
}
