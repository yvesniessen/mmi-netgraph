using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph
{
    public delegate void LoggingEvent(object sender, LogEventArgs a);
    class EventLogger
    {
            public static event LoggingEvent OnLoggingEvent;

            public static void Log(string Text)
            {

                if (OnLoggingEvent != null)
                {
                    LogEventArgs a = new LogEventArgs();
                    a.Text = Text;
                    OnLoggingEvent(null, a);
                }
            }
    }

    public class LogEventArgs
    {
        public string Text;
    }
}
