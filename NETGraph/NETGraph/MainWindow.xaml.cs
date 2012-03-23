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
using System.Collections.ObjectModel;

namespace NETGraph
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
     

    public partial class MainWindow : Window
    {

        #region members
        public static ObservableCollection<ViewData> _ViewData = new ObservableCollection<ViewData>();
        public ObservableCollection<ViewData> ViewData
        { get { return _ViewData; } }
        #region constructor

        #endregion
 
        public MainWindow()
        {
            InitializeComponent();
            richTextBoxLog.AppendText("NetGraph Version 1.0 Alpha 1");
            EventLogger.writeIntoLogFile("program start ");
            registerEvents();
            _ViewData.Add(new ViewData { Vertex = "v1", Edges = "e1", Costs = "42" });
        }
        ~MainWindow()
        {
            unRegisterEvents();
            EventLogger.writeIntoLogFile("program closed ");
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

            foreach (Vertex<String> v in _graph.Vertexes)
            {
                foreach (Edge ed in v.Edges)
                {
                    Debug.Print("Vertex: " + v.VertexName.ToString() +
                                " --EDGE: " + ed.EdgeName + " START: " +
                                ed.StartVertex.VertexName.ToString() + " ENDE: " +
                                ed.EndVertex.VertexName.ToString());
                }
            }

            GraphList depthsarchgraph = _graph.depthsearch(_graph.findVertex("0"));

            foreach (String s in depthsarchgraph.Vertexes)
               Console.WriteLine(s);

        }
        private void menuHelpLogFileOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("notepad", "errorlog.txt");
            }catch(Exception ex) {System.Windows.Forms.MessageBox.Show("LogFile does not exist!\n"+ex.Message.ToString());}
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

        public class ViewData
        {
            public String Vertex {get;set;}
            public String Edges {get;set;}
            public String Costs {get;set;}
        }
}
