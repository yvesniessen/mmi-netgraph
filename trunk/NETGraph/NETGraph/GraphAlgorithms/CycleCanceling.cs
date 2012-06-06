using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;

namespace NETGraph.GraphAlgorithms
{
    class CycleCanceling : IGraphAlgorithm
    {
        FordFulkerson m_fordFulk = new FordFulkerson();
        MooreBellmanFord m_MooreBellmannFord = new MooreBellmanFord();
        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Graph result = new Graph();

            /*graph.findEdge(graph.findVertex("0"), graph.findVertex("2")).Flow = 2;
            graph.findEdge(graph.findVertex("0"), graph.findVertex("1")).Flow = 3;
            graph.findEdge(graph.findVertex("1"), graph.findVertex("2")).Flow = 5;
            graph.findEdge(graph.findVertex("2"), graph.findVertex("3")).Flow = 1;
            graph.findEdge(graph.findVertex("2"), graph.findVertex("4")).Flow = 4;
            graph.findEdge(graph.findVertex("2"), graph.findVertex("5")).Flow = 2;
            graph.findEdge(graph.findVertex("5"), graph.findVertex("4")).Flow = 0;*/

            graph = buildSuperTargetandSource(graph);
            //Ford Fulkerson für initialen Fluss
            graph = m_fordFulk.performAlgorithm(graph, graph.findVertex("S*"));
            
            Graph residualGraph = m_fordFulk.buildResidualGraph(graph);
            //negativen Cycle suchen
            Graph negativeCycle = findNegativeCycle(residualGraph, startVertex);

            graph = deleteSuperTargetandSource(graph);

            //solange Negativer Cycle noch da
            while (negativeCycle != null)
            {
                //Flaschenhals finden im Cycle
                double min_costs = m_fordFulk.getMinCostsFromEdges(negativeCycle);

                // Suche alle Kanten im Residualgraphen und passe die Kapazitäten im ursprünglichen Graphen an
                foreach (Edge edge in negativeCycle.Edges)
                {
                    Edge currentedge = graph.findEdge(edge);
                    if (currentedge != null)
                    {
                        currentedge.Flow += min_costs;
                    }
                    else
                    {
                        //wenn Kante nicht gefunden suche die Umgedreht
                        currentedge = graph.findInvertedEdge(edge);
                        if (currentedge != null)
                            currentedge.Flow -= min_costs;
                    }
                }
                //Residualgraph für nächsten durchlauf
                residualGraph = m_fordFulk.buildResidualGraph(graph);
                //Nächsten negativen Cycle finden
                negativeCycle = findNegativeCycle(residualGraph, startVertex);
            }

            double min_flow_costs = 0;
            foreach (Edge edge in graph.Edges)
            {
                min_flow_costs += edge.RealCosts * edge.Flow;
            }

            EventManagement.GuiLog("Kosten des minimalen Flusses: " + min_flow_costs.ToString());

            return graph;
        }

        private Graph deleteSuperTargetandSource(Graph graph)
        {
            Vertex<String> superSource = graph.findVertex("S*");
             Vertex<String> superTarget = graph.findVertex("T*");
             foreach (Vertex<String> vert in superSource.findNeighbors(false))
            {
                graph.deleteEdge(superSource, vert);
            }

            foreach (Vertex<String> vert in superTarget.findNeighbors(false))
            {
                graph.deleteEdge(vert, superTarget);
            }

            graph.deleteVertex(superSource);
            graph.deleteVertex(superTarget);

            return graph;
        }

        private Graph buildSuperTargetandSource(Graph graph)
        {
            List<Vertex<String>> sources = new List<Vertex<string>>();
            List<Vertex<String>> targets = new List<Vertex<string>>();

            double totalBalance = 0;

            foreach (Vertex<String> vertex in graph.Vertexes)
            {
                if (vertex.Balance > 0)
                {
                    sources.Add(vertex);
                   
                }
                else if (vertex.Balance < 0)
                {
                    targets.Add(vertex);
                }
                totalBalance += vertex.Balance;
            }
            Vertex<String> SuperSource = new Vertex<string>("S*");
            Vertex<String> SuperTarget = new Vertex<string>("T*");

            foreach (Vertex<String> vertex in sources)
            {
                graph.addEdge(SuperSource, graph.findVertex(vertex.VertexName), graph.findVertex(vertex.VertexName).Balance);
            }
            foreach (Vertex<String> vertex in targets)
            {
                graph.addEdge(graph.findVertex(vertex.VertexName), SuperTarget, graph.findVertex(vertex.VertexName).Balance * (-1));
            }

            if (totalBalance == 0)
            {
                return graph;
            }
            else
            {
                EventManagement.GuiLog("Balancen nicht ausgeglichen. Abbruch!");
                return new Graph();
            }
        }

        public Graph findNegativeCycle(Graph graph, Vertex<String> startVertex)
        {
            //Kante des Rattenschwanzes finden
            Edge negativeEdge = m_MooreBellmannFord.getEdgeForNegativeCycle(graph, graph.findVertex(startVertex.VertexName));
            if (negativeEdge != null)
            {
                Graph cycleGraph = new Graph();
                Vertex<String> startVertexForCycle = negativeEdge.StartVertex;
                Vertex<String> currentVertex = startVertexForCycle.PreVertex;
                
                //Cycle solange Rückwärtslaufen lassen bis wieder am Anfang
                while (currentVertex != startVertexForCycle)
                {
                    Edge tempEdge = graph.findEdge(currentVertex.PreVertex, currentVertex);
                    cycleGraph.addEdge(currentVertex.PreVertex, currentVertex, tempEdge.Costs, tempEdge.RealCosts);
                    currentVertex = currentVertex.PreVertex;
                }
                Edge tmpEdge = graph.findEdge(currentVertex.PreVertex, currentVertex);
                cycleGraph.addEdge(currentVertex.PreVertex, currentVertex, tmpEdge.Costs, tmpEdge.RealCosts);

                return cycleGraph;
            }
            return null;
        }


    }
}
