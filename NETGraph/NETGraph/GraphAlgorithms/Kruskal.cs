using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;

namespace NETGraph.GraphAlgorithms
{
    class Kruskal : IGraphAlgorithm
    {
        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Graph T = new Graph();
            Graph temp = new Graph();

            // Kopiere die Vertexliste in den Temp-Graphen
            temp.Vertexes = graph.Vertexes;

            foreach (Vertex<String> v in temp.Vertexes)
            {
                // Entferne alle Kanten aus Temp (um sie später wieder zu füllen)
                foreach (Edge e in v.Edges)
                {
                    temp.deleteEdge(e);
                }
            }

            // Kopiere die Vertexes in T
            T.Vertexes = temp.Vertexes;

            List<Edge> edges = graph.Edges;
            
            edges.Sort(delegate(Edge e1, Edge e2) { return e1.Costs.CompareTo(e2.Costs); });

            while(edges.Count > 0)
            {
                //Hole die günstigste Kante aus der Liste und entferne sie
                Edge currentEdge = edges.First();
                edges.Remove(currentEdge);

                temp.addEdge(currentEdge.StartVertex, currentEdge.EndVertex, currentEdge.Costs);

                //prüfen ob beide Vertexes bereits in selber Komponente
                if (!temp.checkIfTwoVertexesInSameComponent(currentEdge.StartVertex,currentEdge.EndVertex))
                {
                    T.addEdge(currentEdge.StartVertex, currentEdge.EndVertex, currentEdge.Costs);
                }
                else
                {
                    temp.deleteEdge(currentEdge);
                }

                temp.addEdge(currentEdge.StartVertex, currentEdge.EndVertex, currentEdge.Costs);
            }

            return T;
        }

    }
}
