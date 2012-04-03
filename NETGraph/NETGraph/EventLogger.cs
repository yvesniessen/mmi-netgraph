using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace NETGraph
{
    public delegate void LoggingEvent(object sender, LogEventArgs a);
    class EventLogger
    {
            public static event LoggingEvent OnLoggingEvent;

            public static void GuiLog(string Text)
            {

                if (OnLoggingEvent != null)
                {
                    LogEventArgs a = new LogEventArgs();
                    a.Text = Text;
                    OnLoggingEvent(null, a);
                }
            }

            public static void writeIntoLogFile(String logMessage)
            {
                String logFileName = "errorlog.txt";
                // this function provides a stream into the logfile
                if (!File.Exists(@logFileName))
                {
                    FileInfo fi = new FileInfo(logFileName);
                    FileStream fs = fi.Create();
                    fs.Close();
                    Debug.Write("Log Datei wurde angelegt");
                }

                StreamWriter fileWriter = new StreamWriter(logFileName,true);
                
                fileWriter.Write ( System.DateTime.Now.ToString() + ":");
                fileWriter.Write(Environment.UserName.ToString() + ":  ");
                fileWriter.WriteLine (logMessage);
                fileWriter.Close();
            }

            
    }

    public class LogEventArgs
    {
        public string Text;
    }


}
