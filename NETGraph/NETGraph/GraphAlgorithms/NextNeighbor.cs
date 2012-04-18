using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;

namespace NETGraph.GraphAlgorithms
{
    class NextNeighbor : IGraphAlgorithm
    {
        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Graph resultGraph = new Graph();
            
            // Alle Knoten werden benötigt, daher werden sie in den Ergebnisgraphen eingefügt

            int NumOfAllVertex = graph.Vertexes.Count();

            // CurrentVertex = aktueller Knoten auf dem gearbeitet wird
            // StartVertex = wird benötigt um am Ende Kreise zu bilden.
            Vertex<String> currentVertex = startVertex;
            Vertex<String> neighborVertex = startVertex;

            // Solange wie es nicht N-1 Kanten gibt
            while ( resultGraph.Edges.Count() < NumOfAllVertex-1)
            {
                // Füge den Aktuellen Knotenstart in die resultGraph ein
                resultGraph.addVertex(currentVertex);

                // Sortiert die Kantenliste des aktuellen Knotens nach den Kosten
                currentVertex.Edges.Sort(delegate(Edge e1, Edge e2) { return e1.Costs.CompareTo(e2.Costs); });

                // Nimm die günstigeste Kante, welche zu einem noch nicht besuchten Knoten führt                
                foreach (Edge e in currentVertex.Edges)
                {
                    neighborVertex = currentVertex.getNeighborVertex(e);
                    // Falls die aktuelle Kante nicht markiert ist
                    if (!neighborVertex.Marked)
                    {
                        resultGraph.addEdge(currentVertex, neighborVertex);
                        break;
                    }
                }

                // Nimm den letzen hinzugefügten Knoten und Verbinde ihn mit dem Startknoten


            
            }


            return resultGraph;
        }

        private bool checkIfSameVertexes(Graph g1, Graph g2)
        {

            if (g1.Vertexes.Count == g2.Vertexes.Count)
            {
                foreach (Vertex<String> v in g1.Vertexes)
                {
                    if (!g2.Vertexes.Contains(v))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
