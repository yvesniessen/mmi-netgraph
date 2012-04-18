using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows;

namespace NETGraph
{
   

    class EventManagement
    {
        #region timer
        private static Stopwatch _stopwatch;
        public static void startTimer()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            
        }

        public static void stopTimer()
        {
            try
            {
                _stopwatch.Stop();
                Debug.WriteLine("Estimated Timer: " + _stopwatch.ElapsedMilliseconds.ToString());
                GuiLog("Estimated Process Time: " + _stopwatch.ElapsedMilliseconds.ToString());
                GuiLog("Estimated CPU Ticks: " + _stopwatch.ElapsedTicks.ToString());
                TimerLog(_stopwatch.ElapsedMilliseconds.ToString());
                _stopwatch.Reset();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Timer Exception : " +ex.Message.ToString() );
                MessageBox.Show("Timer Exception : " +ex.Message.ToString() );
            }
        }

        public delegate void TimerEvent(object sender, String ms_time);
        public static event TimerEvent OnTimerEvent;
        public static void TimerLog(string ms_time)
        {

            if (OnTimerEvent != null)
            {
                OnTimerEvent(null, ms_time);
            }
        }
        #endregion 



        public delegate void LoggingEvent(object sender, LogEventArgs a);
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
        public delegate void UpdateGuiGraph(object sender, Graph graph);
        public static event UpdateGuiGraph OnUpdateGuiGraph;
        public static void updateGuiGraph(Graph graph)
        {

            if (OnUpdateGuiGraph != null)
            {
                OnUpdateGuiGraph(null, graph);
            }
        }
           
    }

    public class LogEventArgs
    {
        public string Text;
    }


}
