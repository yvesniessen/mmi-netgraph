using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;
using Demo.WpfGraphApplication;
namespace NETGraph.GraphAlgorithms
{
    class HeuristikNN : IGraphAlgorithm
    {
        public Boolean DrawSingleStep = false;
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
            while (resultGraph.Edges.Count() < NumOfAllVertex - 1)
            {
                // Füge den Aktuellen Knotenstart in die resultGraph ein
                resultGraph.addVertex(currentVertex);
                currentVertex.Marked = true;

                // Sortiert die Kantenliste des aktuellen Knotens nach den Kosten
                currentVertex.Edges.Sort(delegate(Edge e1, Edge e2) { return e1.Costs.CompareTo(e2.Costs); });

                // Nimm die günstigeste Kante, welche zu einem noch nicht besuchten Knoten führt                
                foreach (Edge e in currentVertex.Edges)
                {
                    neighborVertex = currentVertex.getNeighborVertex(e);
                    // Falls der aktuelle Knoten nicht markiert ist
                    if (!neighborVertex.Marked)
                    {
                        resultGraph.addEdge(currentVertex, neighborVertex, e.Costs);
                        currentVertex = neighborVertex;
                        break;
                    }
                }
            }

            // Nimm den letzen hinzugefügten Knoten und Verbinde ihn mit dem Startknoten
            resultGraph.addEdge(currentVertex, startVertex, graph.findEdge(currentVertex.VertexName, startVertex.VertexName).Costs);

            return resultGraph;
        }
    }
}
