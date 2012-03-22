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
        private List<Edge> edges = new List<Edge>();
        private List<Vertex<String>> vertexes = new List<Vertex<String>>();
        private int _collisionOfVertexes = 0;
        private int _collisionOfEdges = 0;
        private bool _parallelEdges = false;
        private bool _directedEdges = true;
        private int _numberOfVertexes = 0;
        #endregion

        #region properties
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

        #endregion

        #region public functions
        public Edge checkEdgeExists(Vertex<String> start, Vertex<String> end)
        {
            foreach (Edge edge in edges)
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
            return this.vertexes;
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
                        edges.Add(tempEdge);
                    }
                }
                else
                {
                    if ((checkEdgeExists(start, end) == null))
                    {
                        Edge tempEdge = new Edge(start, end);
                        edges.Add(tempEdge);
                    }
                }
            }
            else
            {
                if (!DirectedEdges)
                {
                    //TO FIX ?? Fall mal prüfen...

                    Edge tempEdge = new Edge(start, end);
                    edges.Add(tempEdge);
                }
                else
                {
                    Edge tempEdge = new Edge(start, end);
                    edges.Add(tempEdge);
                }
            }
        }

        public List<Edge> getEdges()
        {
            return edges;
        }

        public Vertex<String> addVertex(Vertex<String> vertex)
        {
            bool check = false;

            if (!this.vertexes.Contains(vertex))
            {
                foreach (Vertex<String> v in vertexes)
                {
                    if (v.VertexName.Equals(vertex.VertexName))
                    {
                        check = true;
                        //MessageBox.Show("Knoten schon da!");
                        System.Diagnostics.Debug.WriteLine("Knoten schon da: " + vertex.VertexName.ToString());
                        CollisionOfVertexes++;
                        return v;
                    }
                }
                if (!check)
                {
                    this.vertexes.Add(vertex);
                    return vertex;
                }
            }
            return null;
        }
        #endregion
    }
}
