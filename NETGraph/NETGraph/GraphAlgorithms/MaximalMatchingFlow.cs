using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;

namespace NETGraph.GraphAlgorithms
{
    class MaximalMatchingFlow : IGraphAlgorithm
    {
        List<Vertex<String>> sources = new List<Vertex<string>>();
        List<Vertex<String>> targets = new List<Vertex<string>>();

        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            graph = init(graph);

            Graph tempGraph = graph;

            IGraphAlgorithm FordFulkerson = new FordFulkerson();
            for (int i = 0; i < targets.Count; i++)
            {
                tempGraph = FordFulkerson.performAlgorithm(graph, graph.findVertex("S*"));
            }

            graph = deleteSuperTargetandSource(graph);

            return graph;
        }

        private Graph init(Graph graph)
        {
            graph.DirectedEdges = true;


            foreach (Vertex<String> vertex in graph.Vertexes)
            {
                //Nimm nur Start-Vertexes
                if (vertex.Balance > 0)
                {
                    sources.Add(vertex);
                    foreach (Edge e in vertex.Edges)
                    {

                        e.Flow = 0;
                        e.Costs = 1;
                        e.RealCosts = 1;

                        if (e.EndVertex.VertexName.Equals(vertex.VertexName))
                        {
                            e.EndVertex = e.StartVertex;
                        }

                        e.StartVertex = vertex;
                    }
                }
                else
                {
                    targets.Add(vertex);
                }
            }

            if (sources.Count == 0 || targets.Count == 0 )//|| sources.Count < targets.Count)
            {
                EventManagement.GuiLog("FEHLER FEHLER ABBRUCH ABBRUCH");
                return new Graph();
            }

            graph = buildSuperTargetandSource(graph);
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

            
                return graph;
            
        }
    }
}
