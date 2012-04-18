using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph
{

    //ToDO: Change Generics to template!
    class Edge : IComparable
    {
        #region members
        private double _costs = 0;
        private bool _marked = false;
        private String _edgeName;      
        private Vertex<string> _startVertex, _endVertex;

        // The value holder for compareTo-Implementation
        protected int m_value = 0;

        #endregion

        #region constructors
        public Edge(Vertex<string> start, Vertex<string> end)
        {
            StartVertex = start;
            EndVertex = end;
            EdgeName = StartVertex.ToString() + ":" + EndVertex.ToString();

            StartVertex.Edges.Add(this);
            EndVertex.Edges.Add(this);
        }
        #endregion

        # region properties
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

        public bool Marked
        {
            get
            {
                return _marked;
            }
            set
            {
                _marked = value;
            }
        }

        public String EdgeName
        {
            get
            {
                return _edgeName;
            }
            set
            {
                _edgeName = value;
            }
        }

        public Vertex<string> StartVertex
        {
            get
            {
                return _startVertex;
            }
            set
            {
                _startVertex = value;
            }
        }

        public Vertex<string> EndVertex
        {
            get
            {
                return _endVertex;
            }
            set
            {
                
                _endVertex = value;
            }
        }
        #endregion

        #region public functions
        public override String ToString()
        {
            return "Edge: " + _edgeName.ToString() + " Startknoten: " + this._startVertex.ToString() + " Endknoten: "+  this._endVertex.ToString() + " Kosten: " + this._costs.ToString();
        }
        
        public int CompareTo(Object obj)
        {
            if (obj is Edge)
            {
                Edge temp = (Edge)obj;

                return m_value.CompareTo(temp.m_value);
            }

            throw new ArgumentException("object is not an Edge");
        }
        #endregion
    }
}
