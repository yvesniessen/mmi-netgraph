using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;
using Demo.WpfGraphApplication;
namespace NETGraph.GraphAlgorithms
{
    class HeuristikDS : IGraphAlgorithm
    {
        public Boolean DrawSingleStep = false;
        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            // Ansatz:
            // 1. Spanne einen MST
            // 2. Gehe durch den Graphen mit einer Tiefensuche
            // 3. Merke die die Reihenfolge der Knoten
            // 4. Verbinde immer die Kanten zweier Aufeinanderfolgender Knoten
            // 5. Verdinde den letzen und ersten Knoten zu einer Rundreise

            

            IGraphAlgorithm m_graphAlgorithm_Kruskal = new Kruskal();
            Graph MST = m_graphAlgorithm_Kruskal.performAlgorithm(graph, graph.Vertexes.First());

            MST.unmarkGraph();
            IGraphAlgorithm m_graphAlgorithm_Depthsearch = new DepthSearch();
            Graph DS = m_graphAlgorithm_Depthsearch.performAlgorithm(MST, MST.Vertexes.First());

            Graph resultGraph = new Graph();
            Edge e;

                for (int i = 0;i < DS.Vertexes.Count()-1 ; i++)
                {
                    e = graph.findEdge(DS.Vertexes.ElementAt(i).VertexName, DS.Vertexes.ElementAt(i + 1).VertexName);

                    resultGraph.addEdge(new Vertex<String>(e.StartVertex.VertexName), new Vertex<String>(e.EndVertex.VertexName), e.Costs);
                }

            e = graph.findEdge(DS.Vertexes.ElementAt(DS.Vertexes.Count()-1).VertexName, DS.Vertexes.ElementAt(0).VertexName);

            resultGraph.addEdge(new Vertex<String>(e.StartVertex.VertexName), new Vertex<String>(e.EndVertex.VertexName), e.Costs);

            return resultGraph;
        }
    }
}
