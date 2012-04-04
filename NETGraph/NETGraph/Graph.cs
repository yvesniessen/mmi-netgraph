using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NETGraph.Algorithm;

namespace NETGraph
{
    class Graph
    {
        #region members
        private String _graphName;
        private List<Edge> _edges = new List<Edge>();
        private List<Vertex<String>> _vertexes = new List<Vertex<String>>();
        private List<Graph> _connectingComponents = new List<Graph>();
        private int _collisionOfVertexes = 0;
        private int _collisionOfEdges = 0;
        private bool _parallelEdges = false;
        private bool _directedEdges = false;
        private int _numberOfVertexes = 0;
        private IGraphAlgorithm m_graphAlgorithm;
        private double _processTime = .0;
        #endregion

        #region constructors
        public Graph()
        {
           
        }
        
        public Graph(String graphName)
        {
            GraphName = GraphName;
        }
        #endregion

        #region properties
        public double ProcessTime
        {
            get
            {
                return _processTime;
            }
            set
            {
                EventManagement.GuiLog("process time calculated: " + ProcessTime);
                _processTime = value;
            }
        }

        public String GraphName 
        { 
            get
            {
                return _graphName;
            }
            set
            {
                _graphName = value;
            }
        }

        //@SD: Ist der Getter so okay?
        public List<Graph> ConnectingComponents
        {
            get
            {
                _connectingComponents = getConnectingComponents();
                return _connectingComponents;
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

        #region private functions

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
            EventManagement.GuiLog("Es wurde keine Edge von " + startVertex.VertexName.ToString() + " nach " + endVertex.VertexName.ToString() + " gefunden.");
            return null;
        }
        #endregion

        #region public functions
        public void updateGUI()
        {
            EventManagement.updateGuiGraph(this);
        }
        public Edge checkEdgeExists(Vertex<String> start, Vertex<String> end)
        {
            foreach (Edge edge in Edges)
            {
                if ((edge.StartVertex.VertexName == start.VertexName && edge.EndVertex.VertexName == end.VertexName))
                {
                    //MessageBox.Show("Edge schon vorhanden: " + e.ToString());
                    System.Diagnostics.Debug.WriteLine("Edge schon vorhanden: " + edge.ToString());
                    
                    return edge;
                }
            }
            return null;
        }

        public void addEdge(Vertex<String> startVertex, Vertex<String> endVertex)
        {
            startVertex = addVertex(startVertex);
            endVertex = addVertex(endVertex);
         
            if (!ParallelEdges)
            {
                if (!DirectedEdges)
                {
                    if ((checkEdgeExists(startVertex, endVertex) == null) && (checkEdgeExists(endVertex, startVertex) == null))
                    {
                        Edges.Add(new Edge(startVertex, endVertex));
                    }
                    else
                    {
                        _collisionOfEdges++;
                    }
                }
                else
                {
                    if ((checkEdgeExists(startVertex, endVertex) == null))
                    {
                        Edges.Add(new Edge(startVertex, endVertex));
                    }
                    else
                    {
                        _collisionOfEdges++;
                    }
                }
            }
            else
            {
                if (!DirectedEdges)
                {
                    //TODO: Diesen Fall mal überprüfen
                    Edges.Add(new Edge(startVertex, endVertex));
                }
                else
                {
                    Edges.Add(new Edge(startVertex, endVertex));
                }
            }
        }
        
        public void addEdge(Vertex<String> startVertex, Vertex<String> endVertex, double edgecosts)
        {
            startVertex = addVertex(startVertex);
            endVertex = addVertex(endVertex);


            if (!ParallelEdges)
            {
                if (!DirectedEdges)
                {
                    if ((checkEdgeExists(startVertex, endVertex) == null) && (checkEdgeExists(endVertex, startVertex) == null))
                    {
                        Edge tempEdge = new Edge(startVertex, endVertex);
                        tempEdge.Costs = edgecosts;
                        Edges.Add(tempEdge);
                    }
                    else
                    {
                        _collisionOfEdges++;
                    }
                }
                else
                {
                    if ((checkEdgeExists(startVertex, endVertex) == null))
                    {
                        Edge tempEdge = new Edge(startVertex, endVertex);
                        tempEdge.Costs = edgecosts;
                        Edges.Add(tempEdge);
                    }
                    else
                    {
                        _collisionOfEdges++;
                    }
                }
            }
            else
            {
                if (!DirectedEdges)
                {
                    //TO FIX ?? Fall mal prüfen...

                    Edge tempEdge = new Edge(startVertex, endVertex);
                    tempEdge.Costs = edgecosts;
                    Edges.Add(tempEdge);
                }
                else
                {
                    Edge tempEdge = new Edge(startVertex, endVertex);
                    tempEdge.Costs = edgecosts;
                    Edges.Add(tempEdge);
                }
            }
        }
        
        public Vertex<String> addVertex(Vertex<String> vertex)
        {
            bool check = false;

            if (!this.Vertexes.Contains(vertex))
            {

                foreach (Vertex<String> v in _vertexes)
                {
                    //Zuerst prüfen, ob der Knoten schon in der Vertex-Liste vorhanden ist
                    if (v.VertexName.Equals(vertex.VertexName))
                    {
                        check = true;
                        System.Diagnostics.Debug.WriteLine("Knoten schon da: " + vertex.VertexName.ToString());
                        CollisionOfVertexes++;
                        return v;
                    }
                }
                //Wenn nicht da: füge ihn hinzu!
                if (!check)
                {
                    this.Vertexes.Add(vertex);
                    return vertex;
                }
            }
            //Checken!
            return vertex;
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

        public Edge findEdge(String startVertex, String endVertex)
        {
            Vertex<String> _startVertex = findVertex(startVertex);
            Vertex<String> _endVertex = findVertex(endVertex);

            if ((_startVertex != null) && (_endVertex != null))
            {
                return getEdge(_startVertex, _endVertex);
            }
            else
            {
                return null;
            }
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

        public List<Graph> getConnectingComponents()
        {
            m_graphAlgorithm = new BreathSearch();
            foreach (Vertex<String> vertex in Vertexes)
            {
                if ((!vertex.Marked) ) //&& (vertex.Edges.Count > 0))
                {
                    _connectingComponents.Add(m_graphAlgorithm.performAlgorithm(this, vertex));
                }
            }

            return _connectingComponents;
        }

        public void unmarkGraph()
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

        public bool deleteEdge(Edge edge)
        {
            //Wenn Kante in Liste der Kanten
            if (Edges.Contains(edge))
            {
                //Lösche Kante aus Liste der Kanten
                Edges.Remove(edge);
                //Durchsuche Liste der Knoten nach der Kante
                foreach(Vertex<String> v in Vertexes)
                {
                    //Wenn Kante in der Liste der Kanten des Knotens
                    if (v.Edges.Contains(edge))
                    {
                        //Lösche Kante aus der Liste des Knotens
                        v.Edges.Remove(edge);
                    }
                }
                return true; //Kante ist gelöscht und aus allen Knoten entfernt
            }
            else return false; //Kante nicht gefunden
        }

        public bool deleteEdge(Vertex<String> startVertex, Vertex<String> endVertex)
        {
            //TODO
            Edge _edge = getEdge(startVertex, endVertex);

            if (_edge != null)
            {
                if (deleteEdge(_edge))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool deleteEdge(String startVertex, String endVertex)
        {
            Vertex<String> _startVertex;
            Vertex<String> _endVertex;

            //Suche Start&Endknoten
            _startVertex = findVertex(startVertex);
            _endVertex = findVertex(endVertex);

            if ((_startVertex != null) && (_endVertex != null))
            {
                if (deleteEdge(_startVertex, _endVertex))
                {
                    return true; //Kante wurde gelöscht
                }
                else
                {
                    return false; //Kante wurde nicht gelöscht
                }
            }
            else return false; //Einer der Knoten nicht gefunden
        }

        public bool deleteVertex(Vertex<String> delVertex)
        {
            foreach (Vertex<String> v in Vertexes)
            {
                //Zuerst alle Kanten löschen die den Knoten beinhalten
                foreach (Edge e in v.Edges)
                {
                    if ( (e.StartVertex.Equals(v)) || (e.EndVertex.Equals(v)) )
                    {
                        deleteEdge(e);
                    }
                }
                if (v.Equals(delVertex)) 
                {
                    Vertexes.Remove(v);
                    return true;
                }
            }
            return false;
        }

        public bool deleteVertex(String VertexName)
        {
            //Suche Vertex der den VertexName trägt
            Vertex<String> _vertex = findVertex(VertexName);
            //Wenn gefunden
            if (_vertex != null)
            {
                //Lösche den Knoten
                return deleteVertex(_vertex);
            }
            //Nicht gefunden
            else
            {
                return false;
            }
        }

        public String findNearestNeighbor(String startVertex)
        {
            String nearestNeigbor = default(String);
            
            return null;
        }

        #endregion
    }
}
