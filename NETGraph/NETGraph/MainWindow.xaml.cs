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
using NETGraph.Algorithm;
using NETGraph.GraphAlgorithms;
using Demo.WpfGraphApplication;

namespace NETGraph
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public int counter = 1;
        #region constructor
        public MainWindow()
        {
            InitializeComponent();
            richTextBoxLog.AppendText("NetGraph Version 1.0 Alpha 1");
            EventManagement.writeIntoLogFile("program start ");
            registerEvents();

        }
        ~MainWindow()
        {
            unRegisterEvents();
            EventManagement.writeIntoLogFile("program closed ");
        }
        #endregion

        #region members
        public static ObservableCollection<ViewData> _ViewData = new ObservableCollection<ViewData>();
        public ObservableCollection<ViewData> ViewData
        { get { return _ViewData; } }


        public static ObservableCollection<ViewDataVertexes> _ViewDataVertexes = new ObservableCollection<ViewDataVertexes>();
        public ObservableCollection<ViewDataVertexes> ViewDataVertexes
        { get { return _ViewDataVertexes; } }



        private IGraphAlgorithm m_graphAlgorithm;
        private Graph _graph;
        private GraphListData _graphList;
        private Stack<Graph> _graphStateStack = new Stack<Graph>();
        #endregion

        #region un/-register events
        void registerEvents()
        {
            //This event updates an String the gui textfield
            EventManagement.OnLoggingEvent += new EventManagement.LoggingEvent(EventManagement_OnLoggingEvent);
            EventManagement.OnUpdateGuiGraph += new EventManagement.UpdateGuiGraph(EventManagement_OnUpdateGuiGraph);
            EventManagement.OnTimerEvent += new EventManagement.TimerEvent(EventManagement_OnTimerEvent);
        }


        void unRegisterEvents()
        {
            EventManagement.OnLoggingEvent -= new EventManagement.LoggingEvent(EventManagement_OnLoggingEvent);
            EventManagement.OnUpdateGuiGraph -= new EventManagement.UpdateGuiGraph(EventManagement_OnUpdateGuiGraph);
            EventManagement.OnTimerEvent -= new EventManagement.TimerEvent(EventManagement_OnTimerEvent);
        }
        #endregion

        #region react 2 gui events
        private void BreathSearch_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running Breathsearch ...");

            if (_graph != null)
            {
                Debug.Print("--------------");
                Debug.Print("Breitensuche: ");

                m_graphAlgorithm = new BreathSearch();

                //TODO @chris: Diese Funktion liefer nen leeren Graphen zurück!!
                EventManagement.startTimer();
                _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
                EventManagement.stopTimer();

                foreach (Vertex<String> vertex in _graph.Vertexes)
                    Debug.Print(vertex.VertexName.ToString());

                _graphList = Export.showGraph(ref _graph);
                _graph.updateGUI();
                _graph.unmarkGraph();
            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }
        private void DepthSearch_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running Depthsearch ...");

            if (_graph != null)
            {

                Debug.Print("--------------");
                Debug.Print("Tiefensuche: ");
                m_graphAlgorithm = new DepthSearch();
                EventManagement.startTimer();
                _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
                EventManagement.stopTimer();
                foreach (Vertex<String> vertex in _graph.Vertexes)
                    Debug.Print(vertex.VertexName.ToString());

                _graphList = Export.showGraph(ref _graph);
                _graph.updateGUI();
                _graph.unmarkGraph();
            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }
        private void Prim_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running Prim ...");

            Debug.Print("--------------");
            Debug.Print("Prim: ");

            m_graphAlgorithm = new Prim();

            _graph.unmarkGraph();
            EventManagement.startTimer();
            _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
            EventManagement.stopTimer();
            //foreach (Vertex<String> vertex in _graph.Vertexes)
            //    Debug.Print(vertex.VertexName.ToString());

            //_graphList = Export.showGraph(ref _graph);
            _graph.updateGUI();
            _graph.unmarkGraph();
        }
        private void Kruskal_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running Kruskal ...");

            Debug.Print("--------------");
            Debug.Print("Kuskal: ");

            m_graphAlgorithm = new Kruskal();

            EventManagement.startTimer();
            _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
            EventManagement.stopTimer();
            foreach (Vertex<String> vertex in _graph.Vertexes)
                Debug.Print(vertex.VertexName.ToString());

            //_graphList = Export.showGraph(ref _graph);
            _graph.updateGUI();
            _graph.unmarkGraph();
        }
        private void HeuristikNN_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running HeuristikNN ...");

            if (_graph != null)
            {
                if (_graph.is_FullGraph())
                {
                    m_graphAlgorithm = new HeuristikNN();

                    EventManagement.startTimer();
                    _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
                    EventManagement.stopTimer();

                    _graphList = Export.showGraph(ref _graph);
                    _graph.updateGUI();
                    _graph.unmarkGraph();
                }
                else
                    System.Windows.MessageBox.Show("Please Load a FullGraph!");
            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }
        private void HeuristikDS_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running HeuristikNN ...");

            if (_graph != null)
            {
                if (_graph.is_FullGraph())
                {
                    m_graphAlgorithm = new HeuristikDS();

                    EventManagement.startTimer();
                    _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
                    EventManagement.stopTimer();

                    _graphList = Export.showGraph(ref _graph);
                    _graph.updateGUI();
                    _graph.unmarkGraph();
                }
                else
                    System.Windows.MessageBox.Show("Please Load a FullGraph!");
            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }
        private void Output_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Draw_Click(object sender, RoutedEventArgs e)
        {
            Window1 draw = new Window1(_graph);
            draw.Title = "Graph - Manuelle Zeichnung " + counter.ToString();
            draw.Show();
            counter++;
        }

        private void Allways_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running AllWays ...");

            if (_graph != null)
            {
                if (_graph.is_FullGraph())
                {
                    m_graphAlgorithm = new HeuristikDS();

                    EventManagement.startTimer();

                    _graph.findAllWaysForVertex(_graph.Vertexes.First(), new List<Vertex<string>>(), 0);

                    // List<List<Vertex<String>>> temp = _graph.allResults;
                    EventManagement.stopTimer();

                    _graphList = Export.showGraph(ref _graph);
                    _graph.updateGUI();
                    _graph.unmarkGraph();
                }
                else
                    System.Windows.MessageBox.Show("Please Load a FullGraph!");
            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }
        private void Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running Dijkstra ...");

            if (_graph != null)
            {
                m_graphAlgorithm = new Dijkstra();

                EventManagement.startTimer();
                _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.findVertex("0"));
                EventManagement.stopTimer();

                _graphList = Export.showGraph(ref _graph);
                _graph.updateGUI();
                _graph.unmarkGraph();

            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }
        private void MooreBellmanFord_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running MooreBellmanFord ...");

            if (_graph != null)
            {
                m_graphAlgorithm = new MooreBellmanFord();

                EventManagement.startTimer();
                String startVertexName = "2";
                _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.findVertex(startVertexName));
                EventManagement.stopTimer();

                EventManagement.GuiLog("Wege von [Knoten " + startVertexName + "]:");
                foreach (Vertex<String> vertex in _graph.Vertexes)
                {
                    EventManagement.GuiLog("[Knoten " + vertex.VertexName + "] Distanz: " + vertex.Costs);
                }

                _graphList = Export.showGraph(ref _graph);
                _graph.updateGUI();
                _graph.unmarkGraph();

            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }
        private void FordFulkerson_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running FordFulkerson ...");
            if (_graph != null)
            {
                m_graphAlgorithm = new FordFulkerson();


                EventManagement.startTimer();
                String startVertexName = "0";
                (m_graphAlgorithm as FordFulkerson).EndVertex = _graph.findVertex("7");

                _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.findVertex(startVertexName));

                EventManagement.stopTimer();

                EventManagement.GuiLog("Wege von [Knoten " + startVertexName + "]:");
                foreach (Edge edge in _graph.Edges)
                {
                    EventManagement.GuiLog("[Kante S:" + edge.StartVertex.VertexName + " E: " + edge.EndVertex.VertexName +"] Kap: " + edge.Costs + " / Flow: " + edge.Flow );
                }

                _graphList = Export.showGraph(ref _graph);
                _graph.updateGUI();
                _graph.unmarkGraph();
            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }
        private void CycleCanceling_Click(object sender, RoutedEventArgs e)
        {
             EventManagement.GuiLog("running CycleCanceling ...");


             if (_graph != null)
             {
                 
                     m_graphAlgorithm = new CycleCanceling();

                     EventManagement.startTimer();
                     _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
                     EventManagement.stopTimer();

                     _graphList = Export.showGraph(ref _graph);
                     _graph.updateGUI();
                     _graph.unmarkGraph();
             }
             else
                 System.Windows.MessageBox.Show("Please Load a Graph!");
        }

        private void SuccessiveShortestPath_Click(object sender, RoutedEventArgs e)
        {
            EventManagement.GuiLog("running SuccessiveShortestPath ...");

            if (_graph != null)
            {

                m_graphAlgorithm = new SuccessiveShortestPath();

                EventManagement.startTimer();
                _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
                EventManagement.stopTimer();

                _graphList = Export.showGraph(ref _graph);
                _graph.updateGUI();
                _graph.unmarkGraph();

                double Costs = 0;

                foreach (Edge edge in _graph.Edges)
                {
                    Costs += edge.Flow * edge.RealCosts;
                }

                EventManagement.GuiLog("Kosten: " + Costs + "\n");
            }
            else
                System.Windows.MessageBox.Show("Please Load a Graph!");
        }


        private void buttonStateRecover_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _graph = _graphStateStack.Pop();
                //Label Ausgabe inkrementieren
                labelState.Content = (Int32.Parse(labelState.Content.ToString()) - 1).ToString();
                _graph.updateGUI();
            }
            catch
            (InvalidOperationException ex) { System.Windows.MessageBox.Show("There is no Last Graph State \n" + ex.Message.ToString()); }
        }

        private void menuFileOpen_Click(object sender, RoutedEventArgs e)
        {
            _graph = Import.openFileDialog();
            if (_graph == null)
            {
                EventManagement.GuiLog("Problem beim einlesen des Graphens -> Abbruch");
                return;
            }


            GraphListData _graphList;
            saveGraphState(_graph);
            _graphList = Export.showGraph(ref _graph);
            _graph.updateGUI();

           

            #region Praktikum 2

            //Debug.Print("Praktikum 2: ");
            //Debug.Print("--------------");

            //Debug.Print("Tiefensuche: ");
            //m_graphAlgorithm = new DepthSearch();
            //Graph depthsarchgraph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
            //foreach (Vertex<String> vertex in depthsarchgraph.Vertexes)
            //{
            //    Debug.Print(vertex.VertexName.ToString());
            //}
            //_graph.unmarkGraph();

            //Debug.Print("--------------");

            //Debug.Print("Breitensuche: ");
            //m_graphAlgorithm = new BreathSearch();
            //Graph breathSeach = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());
            //foreach (Vertex<String> vertex in breathSeach.Vertexes)
            //    Debug.Print(vertex.VertexName.ToString());
            //_graph.unmarkGraph();

            //Debug.Print("--------------");

            /*Debug.Print("MST-Kruskal: ");
            Graph krusal = _graph.kruskal();
            foreach (Vertex<String> vertex in krusal.Vertexes)
                Debug.Print(vertex.VertexName.ToString());

            _graph.unmarkGraph();
            Debug.Print("--------------");
            */

            #endregion

            #region Prim


            //_graph.findEdge("0", "3").Costs = 200;
            //_graph.findEdge("0", "2").Costs = 199;
            //_graph.findEdge("0", "1").Costs = 1;
            //_graph.findEdge("3", "2").Costs = 1;

            //m_graphAlgorithm = new Prim();
            //m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());

            #endregion

            #region Auskommentiert um Hr. Hoever bei der Abgabe nicht zu verwirren:

            /*foreach (Vertex<String> v in _graph.Vertexes)
                        {
                            foreach (Edge ed in v.Edges)
                            {
                                Debug.Print("Vertex: " + v.VertexName.ToString() +
                                            " --EDGE: " + ed.EdgeName + " START: " +
                                            ed.StartVertex.VertexName.ToString() + " ENDE: " +
                                            ed.EndVertex.VertexName.ToString());
                            }
                        }*/

            /*GraphListData depthsarchgraph = _graph.depthsearch(_graph.findVertex("0"));
                
            foreach (String s in depthsarchgraph.Vertexes)
               Console.WriteLine(s);*/
            #endregion

        }

        private void menuHelpLogFileOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("notepad", "errorlog.txt");
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show("LogFile does not exist!\n" + ex.Message.ToString()); }
        }
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region react 2 subscribed events

        // update gui process timer
        void EventManagement_OnTimerEvent(object sender, string ms_time)
        {
            labelAlgoTimeValue.Content = ms_time;
        }



        //This event updates an String the gui textfield
        void EventManagement_OnUpdateGuiGraph(object sender, Graph graph)
        {
            Debug.WriteLine("Updating Graph 2 Gui...");
            EventManagement.GuiLog("Updating Graph to GUI ...");
            ViewData.Clear();
            ViewDataVertexes.Clear();
            const int maxcount = 100;

            if (graph.Edges.Count <= maxcount)
            {
                foreach (var item in graph.Edges)
                {
                    _ViewData.Add(new ViewData { StartVertex = item.StartVertex.ToString(), EndVertex = item.EndVertex.ToString(), Costs = item.Costs.ToString() });
                }
            }
            else
            {
                for (int i = 0; i <= maxcount; i++)
                {
                    _ViewData.Add(new ViewData { StartVertex = graph.Edges[i].StartVertex.ToString(), EndVertex = graph.Edges[i].EndVertex.ToString(), Costs = graph.Edges[i].Costs.ToString() });
                }
                _ViewData.Add(new ViewData { StartVertex = "Nur ersten " + maxcount.ToString() + " angezeigt" });
            }
            if (graph.Vertexes.Count <= maxcount)
            {
                foreach (var v in graph.Vertexes)
                {
                    _ViewDataVertexes.Add(new ViewDataVertexes { Vertex = v.ToString(), Costs = v.Costs.ToString() });
                }
            }
            else
            {
                for (int i = 0; i <= maxcount; i++)
                {
                    _ViewDataVertexes.Add(new ViewDataVertexes { Vertex = graph.Vertexes[i].ToString(), Costs = graph.Vertexes[i].Costs.ToString() });
                }
                _ViewDataVertexes.Add(new ViewDataVertexes { Vertex = "Nur ersten " + maxcount.ToString() + " angezeigt" });
            }

            labelVertexesValue.Content = graph.NumberOfVertexes.ToString();
            labelEdgesValue.Content = graph.Edges.Count.ToString();
            labelVertexCollision.Content = graph.CollisionOfVertexes.ToString();
            labelEdgeCollision.Content = graph.CollisionOfEdges.ToString();

            try
            {
                // calculate graph weight
                double graphWeight = .0;
                foreach (Edge edge in _graph.Edges)
                {
                    graphWeight += edge.Costs;
                }
                labelGraphWeight.Content = graphWeight.ToString();
            }
            catch (Exception ex)
            {
                EventManagement.GuiLog("There was a Problem while counting the graph weight:" + ex.Message.ToString());
                System.Windows.MessageBox.Show("There was a Problem while counting the graph weight");
            }

        }

        void EventManagement_OnLoggingEvent(object sender, LogEventArgs a)
        {
            richTextBoxLog.AppendText("\n");
            richTextBoxLog.AppendText(a.Text);
        }

        #endregion

        #region functions
        private void saveGraphState(Graph graph)
        {
            _graphStateStack.Push(graph);
            labelState.Content = (Int32.Parse(labelState.Content.ToString()) + 1).ToString();
        }
        #endregion

        private void MaxminalMatching_Click(object sender, RoutedEventArgs e)
        {
            m_graphAlgorithm = new MaximalMatchingFlow();
            _graph = m_graphAlgorithm.performAlgorithm(_graph, _graph.Vertexes.First());

            EventManagement.GuiLog("Matchings:");
            
            int counter = 0;
            foreach (Edge edge in _graph.Edges)
            {
                if (edge.Flow == 1)
                {
                    EventManagement.GuiLog("Matching von " + edge.StartVertex.VertexName + " mit " + edge.EndVertex.VertexName);
                    counter++;
                }
            }
            EventManagement.GuiLog("Summe: " + counter);
        }









    }

    #region Observer dataClasses
    public class ViewData
    {
        public String StartVertex { get; set; }
        public String EndVertex { get; set; }
        public String Costs { get; set; }
    }

    public class ViewDataVertexes
    {
        public String Vertex { get; set; }
        public String Costs { get; set; }
    }
    #endregion
}
