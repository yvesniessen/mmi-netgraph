using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace NETGraph
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        #region constructor
        public MainWindow()
        {
            InitializeComponent();
            richTextBoxLog.AppendText("NetGraph Version 1.0 Alpha 1");
            registerEvents();
        }
        ~MainWindow()
        {
            unRegisterEvents();
        }
        #endregion

        #region un/-register events
        void registerEvents()
        {
            //This event updates an String the gui textfield
            EventLogger.OnLoggingEvent += new LoggingEvent(EventLogger_OnLoggingEvent);
        }
    
        void unRegisterEvents()
        {
            EventLogger.OnLoggingEvent += new LoggingEvent(EventLogger_OnLoggingEvent);
        }
        #endregion

        #region react 2 gui events
        private void menuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            Graph _graph = Import.openFileDialog();
            GraphList _graphList;
            _graphList = Export.showGraph(ref _graph);
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region react 2 subscribed events
        //This event updates an String the gui textfield
        void EventLogger_OnLoggingEvent(object sender, LogEventArgs a)
        {
            richTextBoxLog.AppendText("\n");
            richTextBoxLog.AppendText(a.Text);
        }
        #endregion


    }
}
