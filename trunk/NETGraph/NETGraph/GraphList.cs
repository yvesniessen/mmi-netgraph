using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph
{
    class GraphList
    {
        #region members
        private List<String> _edges;
        private List<String> _vertexes;
        #endregion

        #region constructors
        public GraphList(List<String> edges, List<String> vertexes)
        {
            Edges = edges;
            Vertexes = vertexes;
        }
        #endregion

        #region properties
        public List<String> Edges
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

        public List<String> Vertexes
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

        #endregion

    }
}
