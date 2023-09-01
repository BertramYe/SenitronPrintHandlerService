using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SenitronPrintHandlerService
{
    internal class LogHelper
    {
        //private string logFileName;

        private string _createdLogFileName;
        private string _logPath;

        public LogHelper(string logFilePath, string logFileName)
        {
            //string logFilePath = @$"c:/senitron/printhandler/";
           
            // check the outout image path
            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }

            // create log file
            StreamWriter writer = File.AppendText(logFilePath + logFileName + ".log");
            writer.Dispose();
            writer.Close();
            _logPath = logFilePath;
            _createdLogFileName = logFileName;
        }


        public void WriteLog(string logContent)
        {
            //string logFilePath = @"c:/senitron/printhandler/";
            //string logFilePath = this.logPath;
            // check the outout image path
            if (!Directory.Exists(_logPath))
            {
                Directory.CreateDirectory(_logPath);
            }

            if (_createdLogFileName == null)
            {
                throw new FileNotFoundException($"log file not found { _logPath } { _createdLogFileName }.");

            }
            else
            {
                DateTime now = DateTime.Now;
                string formattedDateTime = now.ToString("yyyy-MM-dd h:mm tt");
                string toWirteContent = $"{formattedDateTime}  {logContent}";


                // create log files
                using (StreamWriter writer = File.AppendText(_logPath + _createdLogFileName + ".log"))
                {
                    //string logMessage = $"{DateTime.Now} - This is a log message.";
                    writer.WriteLine(toWirteContent);
                    writer.Dispose();
                    writer.Close();
                }

            }

        }





    }
}
