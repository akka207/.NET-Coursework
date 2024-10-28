using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Exam.Data;

namespace Exam.Services
{
    public enum LogLevel
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG,
        TRACE
    }

    public class Logger
    {
        public static Logger Instance { get; private set; }
            = new Logger((LogLevel)Enum.Parse(typeof(LogLevel), Config.Configuration.GetSection("Logger").GetSection("LogLevel").Value, true),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"PManager/Log.txt"));

        private LogLevel _level;
        private string _path;

        public Logger(LogLevel level, string path)
        {
            _level = level;
            _path = path;
        }

        public void FATAL(string message)
        {
            if (_level >= LogLevel.FATAL)
                Log(LogLevel.FATAL, message);
        }
        public void ERROR(string message)
        {
            if (_level >= LogLevel.ERROR)
                Log(LogLevel.ERROR, message);
        }
        public void WARN(string message)
        {
            if (_level >= LogLevel.WARN)
                Log(LogLevel.WARN, message);
        }
        public void INFO(string message)
        {
            if (_level >= LogLevel.INFO)
                Log(LogLevel.INFO, message);
        }
        public void DEBUG(string message)
        {
            if (_level >= LogLevel.DEBUG)
                Log(LogLevel.DEBUG, message);
        }
        public void TRACE(string message)
        {
            if (_level >= LogLevel.TRACE)
                Log(LogLevel.TRACE, message);
        }

        public void Log(LogLevel level, string message)
        {
            File.AppendAllText(_path, $"{level}\t{DateTime.Now}\t{message}\n");
        }
    }
}
