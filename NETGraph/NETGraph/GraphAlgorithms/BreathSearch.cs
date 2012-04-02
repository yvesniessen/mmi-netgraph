using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph.Algorithm
{
    class BreathSearch : IGraphAlgorithm
    {
        #region IGraphAlgorithm Member

        public Graph performAlgorithm(Graph graph, Vertex<String> startVertex)
        {
            Graph result = new Graph();

            List<Vertex<String>> Schlange = new List<Vertex<string>>();

            Schlange.Add(startVertex);

            do
            {
                Vertex<String> vertex = Schlange.First();
                Schlange.Remove(vertex);

                if (!vertex.Marked)
                {
                    vertex.Marked = true;
                    result.Vertexes.Add(vertex);
                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    if (!neighbor.Marked)
                    {
                        Schlange.Add(neighbor);
                    }
                }


            } while (Schlange.Count != 0);

            return result;
        }

        #endregion
    }
}
