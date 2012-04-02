using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph.Algorithm
{
    class DepthSearch : IGraphAlgorithm
    {

        #region IGraphAlgorithm Member

        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Graph result = new Graph();

            Stack<Vertex<String>> stack = new Stack<Vertex<string>>();

            if (!startVertex.Marked)
            {
                startVertex.Marked = true;
                result.Vertexes.Add(startVertex);

                foreach (Vertex<String> v in startVertex.findNeighbors(graph.DirectedEdges))
                {
                    if (!v.Marked && !stack.Contains(v))
                    {
                        stack.Push(v);
                    }
                }

                while (stack.Count != 0)
                {
                    Vertex<String> currentvertex = stack.Pop();

                    Graph tmp2 = performAlgorithm(result, currentvertex);

                    foreach (Vertex<String> s in tmp2.Vertexes)
                    {
                        result.Vertexes.Add(s);
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
