using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace testTensileMachineGraphics
{
    public class Logger
    {
        private static string currentPath = string.Empty;

        public static void CreateLogFile() 
        {
            DateTime dateTime = System.DateTime.Now;
            double year = dateTime.Year;
            double month = dateTime.Month;
            double day = dateTime.Day;

            string monthstr = string.Empty;
            if (month < 10)
            {
                monthstr = "0" + month;
            }
            else 
            {
                monthstr = month.ToString();
            }

            string daystr = string.Empty;
            if (day < 10)
            {
                daystr = "0" + day;
            }
            else
            {
                daystr = day.ToString();
            }

            string captionOfFile = year.ToString() + monthstr + daystr;
            currentPath = Constants.logFilePath + captionOfFile + ".txt";
            if (File.Exists(currentPath) == true)
            {
                return;
            }
            else
            {
                File.Create(currentPath);
            }
        }

        public static void WriteNode(string message, DateTime dateTime) 
        {
            List<string> content = new List<string>();
            string messageWithDate = message + "        " + "(" + dateTime.ToString() + ")";
            content.Add(messageWithDate);
            if (File.Exists(currentPath) == true)
            {
                File.AppendAllLines(currentPath, content);
            }
        }
    }
}
