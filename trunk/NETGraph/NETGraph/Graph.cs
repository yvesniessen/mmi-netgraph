using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System;

namespace NETGraph
{
    class Graph
    {
        #region members
        private String _graphName;
        private List<Edge> _edges = new List<Edge>();
        private List<Vertex<String>> _vertexes = new List<Vertex<String>>();
        private List<GraphListData> _connectingComponents = new List<GraphListData>();
        private int _collisionOfVertexes = 0;
        private int _collisionOfEdges = 0;
        private bool _parallelEdges = false;
        private bool _directedEdges = false;
        private int _numberOfVertexes = 0;
        #endregion

        #region constructors
        public Graph()
        {
            MainWindow._ViewData.Add(new ViewData { Vertex = "v1", Edges = "e1", Costs = "42" });
        }
        
        public Graph(String graphName)
        {
            GraphName = GraphName;
        }
        #endregion

        #region properties
        public String GraphName { get; set; }

        //@SD: Ist der Getter so okay?
        public List<GraphListData> ConnectingComponents
        {
            get
            {
                return getConnectingComponents();
            }
            set
            {
                _connectingComponents = value;
            }
        }

        public int CollisionOfVertexes
        {
            get
            {
                return _collisionOfVertexes;
            }
            set
            {
                _collisionOfVertexes = value;
            }
        }

        public int CollisionOfEdges
        {
            get
            {
                return _collisionOfEdges;
            }
            set
            {
                _collisionOfEdges = value;
            }
        }
        public bool ParallelEdges
        {
            get
            {
                return _parallelEdges;
            }
            set
            {
                _parallelEdges = value;
            }
        }
        public bool DirectedEdges
        {
            get
            {
                return _directedEdges;
            }
            set
            {
                _directedEdges = value;
            }
        }
      
        public int NumberOfVertexes
        {
            get 
            {
                return _numberOfVertexes;
            }
            set
            {
                _numberOfVertexes = value;
            }
        }

        public List<Vertex<String>> Vertexes
        {
            get
            {
                return _vertexes;
            }
            set
            {
                _vertexes = value;
            }
        }

        public List<Edge> Edges
        {
            get
            {
                return _edges;
            }
            set
            {
                _edges = value;
            }
        }

        #endregion

        #region public functions
        public Edge checkEdgeExists(Vertex<String> start, Vertex<String> end)
        {
            foreach (Edge edge in Edges)
            {
                if ((edge.StartVertex.VertexName == start.VertexName && edge.EndVertex.VertexName == end.VertexName))
                {
                    //MessageBox.Show("Edge schon vorhanden: " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("Edge schon vorhanden: " + edge.ToString());
                    _collisionOfEdges++;
                    return edge;
                }
            }
            return null;
        }

         public List<Vertex<String>> getVertexes()
        {
            return this.Vertexes;
        }

         public void addEdge(Vertex<String> start, Vertex<String> end)
        { 
            start = addVertex(start);
            end = addVertex(end);

         
            if (!ParallelEdges)
            {
                if (!_directedEdges)
                {
                    if( (checkEdgeExists(start, end) == null) && (checkEdgeExists(end, start) == null) )
                    {
                        Edge tempEdge = new Edge(start, end);
                        Edges.Add(tempEdge);
                    }
                }
                else
                {
                    if ((checkEdgeExists(start, end) == null))
                    {
                        Edge tempEdge = new Edge(start, end);
                        Edges.Add(tempEdge);
                    }
                }
            }
            else
            {
                if (!DirectedEdges)
                {
                    //TO FIX ?? Fall mal prüfen...

                    Edge tempEdge = new Edge(start, end);
                    Edges.Add(tempEdge);
                }
                else
                {
                    Edge tempEdge = new Edge(start, end);
                    Edges.Add(tempEdge);
                }
            }
        }

        public List<Edge> getEdges()
        {
            return Edges;
        }

        public Vertex<String> addVertex(Vertex<String> vertex)
        {
            bool check = false;

            if (!this.Vertexes.Contains(vertex))
            {

                foreach (Vertex<String> v in _vertexes)
                {
                    if (v.VertexName.Equals(vertex.VertexName))
                    {
                        check = true;
                        System.Diagnostics.Debug.WriteLine("Knoten schon da: " + vertex.VertexName.ToString());
                        CollisionOfVertexes++;
                        return v;
                    }
                }
                if (!check)
                {
                    this.Vertexes.Add(vertex);
                    return vertex;
                }
            }
            return null;
        }

        public Vertex<String> findVertex(String name)
        {
            foreach(Vertex<String> v in Vertexes)
            {
                if (v.VertexName.Equals(name))
                    return v;
            }
            return null;              
        }
            
        public GraphListData depthsearch(Vertex<String> startvertex)
        {
            GraphListData tmp = new GraphListData(new List<string>(), new List<string>());
            
            Stack<Vertex<String>> stack = new Stack<Vertex<string>>();
            
            if (!startvertex.Marked)
            {
                startvertex.Marked = true;
                tmp.Vertexes.Add(startvertex.VertexName);

                foreach (Vertex<String> v in startvertex.findNeighbors(DirectedEdges))
                {
                    if (!v.Marked && !stack.Contains(v))
                    {
                        stack.Push(v);
                    }    
                }

                while (stack.Count != 0)
                {
                    Vertex<String> currentvertex = stack.Pop();

                        GraphListData tmp2 = depthsearch(currentvertex);

                        foreach (String s in tmp2.Vertexes)
                        {
                            tmp.Vertexes.Add(s);
                        }
                    } 
                
                }

            return tmp;

        
        } 

        public List<Edge> getway(String start, String end)
        {
            

            //Vertex<String> startvertex = this.findVertex(start);

            //int lengh = 0;
            //List<Vertex<String>> stack = null;
            ////List<Vertex> = startvertex.nachbarn
            //List<Edge> path = new List<Edge>();

            //while (stack.Count() != 0)
            //{
            //    for (int i = 0; i < size; ++i)
            //    {
                
                
            //    }


            //    int length = 

            //}

            return null;
        }
                
        public GraphListData breathSearch(Vertex<String> startVertex)
        {
            List<String> outputedges = new List<String>();
            List<String> outputvertexes = new List<String>();

            List<Vertex<String>> Schlange = new List<Vertex<string>>();

            Schlange.Add(startVertex);

            do
            {
                Vertex<String> vertex = Schlange.First();
                Schlange.Remove(vertex);

                if (!vertex.Marked)
                {
                    vertex.Marked = true;
                    outputvertexes.Add(vertex.VertexName.ToString());
                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(this.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    if (!neighbor.Marked)
                    {
                        Schlange.Add(neighbor);
                    }
                    //neighbor.Marked = true;
                }

                
            } while (Schlange.Count != 0);
            
            return new GraphListData(outputedges, outputvertexes);

            /*
            //Alle Knoten auf nicht-markiert setzen
            foreach (Vertex<String> vertex in this.Vertexes)
            {
                vertex.Marked = false;
            }

            //Start Knoten in die Liste hauen und als markiert setzen
            outputvertexes.Add(startVertex.VertexName.ToString());
            startVertex.Marked = true;

            //foreach (Vertex<String> vertex in Vertexes)
            //{
                foreach (Vertex<String> neighbor in startVertex.findNeighbors(this.DirectedEdges))
                {
                    if (!neighbor.Marked)
                    {
                        //Schreibe alle Nachbarn, die nicht markiert sind weg und markiere sie als besucht
                        outputvertexes.Add(neighbor.VertexName.ToString());
                        neighbor.Marked = true;
                    }
                }
            //}*/
        }


        /* Zusammenhangskomponenten
         * 
         * Überlegungen: 
         * 
         * Zusammenhangskomponente: mindestens zwei Knoten, die durch eine Kante verbunden sind
         * 
         * Algo: (richtig?)
         * #1 Gehe über alle Knoten
         * #2 Mache von jedem Knoten, der noch nicht besucht ist eine Breitensuche
         * #3 Wenn der Knoten keinen Nachbarn / keine Kante (Edges==null) hat => keine Zusammenhangskomponente
         * #4 Packe jede Zusammenhangskomponente in einer Liste von GraphListen
         * 
         */

        public List<GraphListData> getConnectingComponents()
        {
            List<String> outputedges = new List<String>();
            List<String> outputvertexes = new List<String>();

            foreach (Vertex<String> vertex in Vertexes)
            {
                if ((vertex.Marked == false) && (vertex.Edges.Count > 0))
                {
                    _connectingComponents.Add(breathSearch(this.Vertexes.First()));
                }
            }

            return _connectingComponents;
        }

        /*
         * Euler Wege / Kreise
         * 
         * Überlegungen: 
         * 
         * Euler = Jede Kante darf einmal besucht werden, aber nicht öfters
         * Wenn am Ende der StartKnoten == EndKnoten ist => EulerKreis
         * 
         * ein Graph kann mehrere Euler Wege / Kreis haben (Je nach Anzahl der Zusammenhangskomponenten)
         * 
         * Parameter = Ein Knoten einer Zusammenhangskomponente
         * Algorithmus: Hierholzer
         * 
         * Geht nur bei ungerichteten Graphen!
         *
         */

        public bool hasEulerWay(Vertex<String> vertexFromConnectingComponent)
        {
            //Eingangsvorraussetzungen prüfen: Maximal 2 Grade der Knoten sind ungerade & Graph ist nicht gerichtet
            int countOddGrades = 0;
            foreach(Vertex<String> vertex in Vertexes)
            {
                if((vertex.Grade % 2) > 0 )
                {
                    countOddGrades++;
                }
            }

            if ((DirectedEdges) && (countOddGrades >= 2))
            {
                EventLogger.Log("Graph erfüllt die Vorraussetzungen für einen Euler-Weg/Kreis nicht.");
                return false;
            }

            return false;
        }

        private Edge getEdge(Vertex<String> startVertex, Vertex<String> endVertex)
        {
            foreach (Edge edge in Edges)
            {
                /*
                 * #1 Start=Start und Ende=Ende  => Richtige Edge gefunden
                 * #2 Nur wenn gerichtete Kanten nicht zugelassen sind (sonst gibts den Fall nicht): Start=Ende und Ende=Start => Richtige Edge gefunden
                 */
                if (edge.StartVertex.VertexName.Equals(startVertex.VertexName) && edge.EndVertex.VertexName.Equals(endVertex.VertexName))
                {
                    return edge;
                }
                else if(!DirectedEdges && edge.EndVertex.VertexName.Equals(startVertex.VertexName) && edge.StartVertex.VertexName.Equals(endVertex.VertexName))
                {
                    return edge;
                }
            }
            EventLogger.Log("Es wurde keine Edge von " + startVertex.VertexName.ToString() + " nach " + endVertex.VertexName.ToString() + " gefunden.");
            return null;
        }

        private void unmarkGraph()
        {
            foreach (Vertex<String> vertex in Vertexes)
            {
                vertex.Marked = false;
            }

            foreach (Edge edge in Edges)
            {
                edge.Marked = false;
            }
        }

        //NEEDS FIX A: Funktioniert noch nicht.

        private List<Vertex<String>> findWay(Vertex<String> from, Vertex<String> to)
        {
            /*
             * Eventuelle Markierungen aufheben!
             */
            unmarkGraph();

            List<Vertex<String>> way = new List<Vertex<string>>();
            List<Vertex<String>> neighbors = new List<Vertex<string>>();
            Vertex<String> current = from;

            do
            {
                way.Add(current);
                neighbors = current.findNeighbors(DirectedEdges);

                current.Marked = true;

                foreach (Vertex<String> neighbor in neighbors)
                {
                    //Nehme die Edge zwischen dem Current und einem Neighbor und checke ob die Edge schon besucht worden ist:
                    Edge currentEdge = getEdge(current, neighbor);

                    if (!neighbor.Marked && !currentEdge.Marked)
                    {
                        current = neighbor;
                        current.Marked = true;
                        currentEdge.Marked = true;
                        way.Add(current);
                        break;
                    }
                }

            } while (current != to);

            return way;
        }

        #endregion
    }
}
