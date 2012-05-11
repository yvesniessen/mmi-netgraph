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
            List<Vertex<String>> minimalerWeg = new List<Vertex<string>>();
            double min_costs = 0;

            // An Anfang enspricht der Residualgraph dem ursprünglichem Graphen das alle Flüsse noch 0 sind
            Graph residualGraph = graph;

            // Solange wie es einen Fluss noch von startVertex nach endVertex gibt ist die Optimierung noch nicht beendet
            List<Vertex<String>> tempWay = residualGraph.getWayForVertexes(startVertex, endVertex);
            while (tempWay.Count() > 0)
            {
                

                // Erstelle einen Residualgraphen
                residualGraph = this.buildResidualGraph(graph, startVertex, endVertex);

                residualGraph.unmarkGraph();
                graph.unmarkGraph();

                // Wende die Breitensuche auf dem Residualgraphen an und finde den MINIMALEN Weg
                minimalerWeg = residualGraph.getWayForVertexes(startVertex, endVertex);

                // Finden die Kanten mit den geringsten Kosten (Flaschenhals) im Weg
                min_costs = getMinCostsFromEdges(residualGraph, minimalerWeg);

                // Suche alle Kanten im Residualgraphen und passe die Kapazitäten im ursprünglichen Graphen an
                for (int i = 0; i < minimalerWeg.Count; i++)
                {
                    Edge tempEdge = graph.findEdge(minimalerWeg[i + 1].VertexName, minimalerWeg[i].VertexName);
                    if (tempEdge != null)
                    {
                        tempEdge.Flow += min_costs;
                    }
                    else
                    {
                        try
                        {
                            tempEdge = graph.findEdge(minimalerWeg[i].VertexName, minimalerWeg[i + 1].VertexName);
                            tempEdge.Flow -= min_costs;
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    i++;
                }
            
            }

            return null;
        }

        private Graph buildResidualGraph(Graph graph, Vertex<String> startVertex, Vertex<String> endVertex)
        {
            Graph residualGraph = new Graph();

            foreach (Edge e in graph.Edges)
            {
                Vertex<String> startVertexForNewEdge = new Vertex<string>(e.StartVertex.VertexName);
                Vertex<String> endVertexForNewEdge = new Vertex<string>(e.EndVertex.VertexName);

                if((e.Costs - e.Flow) != 0)
                {
                    residualGraph.addEdge(startVertexForNewEdge, endVertexForNewEdge, (e.Costs - e.Flow));
                }
                
                if(e.Flow != 0)
                {
                    residualGraph.addEdge(endVertexForNewEdge, startVertexForNewEdge, e.Flow);
                }
            }
            return residualGraph;
        }

        private double getMinCostsFromEdges(Graph graph, List<Vertex<String>> vertexes)
        {
            double costs = graph.findEdge(vertexes[1].VertexName, vertexes[2].VertexName).Costs;
            for (int i = 0; i < vertexes.Count; i++)
            {
                Edge tempEdge = graph.findEdge(vertexes[i].VertexName, vertexes[i + 1].VertexName);
                if (tempEdge.Costs < costs)
                    costs = tempEdge.Costs;

                i++;
            }
            return costs;
        }
    }
}
