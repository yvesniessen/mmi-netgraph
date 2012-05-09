using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;

namespace NETGraph.GraphAlgorithms
{
    class FordFulkerson : IGraphAlgorithm
    {

        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Vertex<String> endVertex = graph.Vertexes.Last();
            /////////////////////////////////////////////////////////////////////////////////////////
            // Schritt 1: Alle ResidualKosten auf 0 setzen - siehe Algovorschrift
            /////////////////////////////////////////////////////////////////////////////////////////
            foreach (Edge e in graph.Edges)
            {
              //  e.Residual_Costs = 0;
            }

            /////////////////////////////////////////////////////////////////////////////////////////
            // Schritt 2: einen beliebigen Weg in G identifizieren und min(e) raussuchen
            /////////////////////////////////////////////////////////////////////////////////////////
            double min_costs = 0;
            List<Vertex<String>> wayFromStartToEnd = new List<Vertex<string>>();
            wayFromStartToEnd = graph.getWayForVertexes(startVertex, endVertex);


            return null;
        }
    }
}
