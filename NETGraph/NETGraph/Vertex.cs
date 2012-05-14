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

namespace NETGraph
{
    class Vertex<T>
    {
        #region members 
        private T _vertexName;
        private double _costs = 0;
        private List<Edge> _edges = new List<Edge>();
        private Vertex<T> _neighborvertex= null;  // Für Dijkstra angelegt
        private Color _color = Colors.White;
        public bool _marked = false;
        private int _grade = 0;
        //Vorgänger
        private Vertex<String> _preVertex;
        #endregion

        #region constructors
        
        public Vertex(T name)
        {
            VertexName = name;
        }
        
        #endregion

        #region properties

        public Vertex<String> PreVertex
        {
            get
            {
                return _preVertex;
            }
            set
            {
                _preVertex = value;
            }
        }

        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }

        public Vertex<T> Neighborvertex
        {
            get
            {
                return _neighborvertex;
            }
            set
            {
                _neighborvertex = value;
            }
        }

        public T VertexName
        {
            get
            {
                return _vertexName;
            }
            set
            {
                _vertexName = value;
            }
        }

        public bool Marked
        {
            get;
            set;
        }

        public double Costs
        {
            get
            {
                return _costs;
            }
            set
            {
                _costs = value;
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

        public int Grade
        {
            get
            {
                _grade = _edges.Count;
                return _grade;
            }
        }

        #endregion

        #region public functions

        public List<Vertex<String>> findNeighbors(bool directedEdges)
        {
            List<Vertex<String>> neighbors = new List<Vertex<string>>();

            if (directedEdges)
            {
                foreach(Edge e in this.Edges)
                {
                    //TODO: schleifen werden nicht berücksichtigt, aber unten werden sie beachtet
                    if (e.EndVertex.VertexName.ToString() != this.VertexName.ToString())
                    {
                        neighbors.Add(e.EndVertex);
                    }
                }
            }
            else
            {
                foreach (Edge e in this.Edges)
                {
                    if (e.StartVertex.VertexName.ToString() == this.VertexName.ToString())
                    {
                        neighbors.Add(e.EndVertex);
                    }
                    else if (e.EndVertex.VertexName.ToString() == this.VertexName.ToString())
                    {
                        neighbors.Add(e.StartVertex);
                    }
                    else
                    {
                        //Exception?!
                    }
                }
            }

            return neighbors;
        }

        public Vertex<String> getNeighborVertex(Edge currentEdge)
        {
            //Überprüfe ob der aktuelle Knoten als start- oder endvertex eingetragen ist und gib den anderen Knoten zurück

            if (currentEdge.StartVertex.VertexName.ToString() != this.VertexName.ToString())
            {
                return currentEdge.StartVertex;
            }
            else
            {
                return currentEdge.EndVertex;
            }
        }

        public override String ToString()
        {
            String s = "V: " + VertexName.ToString()
                        //+ " " + _marked.ToString()
                        + " " + Costs.ToString();

            if (_neighborvertex != null)
            {
                s += " " + _neighborvertex.VertexName.ToString();
            }
            else
            {
                s += " null";
            }

            return s;
        }
    }

        #endregion
}
