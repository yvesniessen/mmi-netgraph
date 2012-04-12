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
                v.Edges.Clear();
            }

            //// Kopiere die Vertexes in T
            //T.Vertexes = temp.Vertexes;

            List<Edge> edges = graph.Edges;
            
            edges.Sort(delegate(Edge e1, Edge e2) { return e1.Costs.CompareTo(e2.Costs); });

            while(edges.Count > 0)
            {
                //Hole die günstigste Kante aus der Liste und entferne sie
                Edge currentEdge = edges.First();
                edges.Remove(currentEdge);

                
                

                //prüfen ob beide Vertexes bereits in selber Komponente
                Vertex<String> StartVertex = temp.findVertex(currentEdge.StartVertex.VertexName);
                Vertex<String> EndVertex = temp.findVertex(currentEdge.EndVertex.VertexName);

                if (!temp.checkIfTwoVertexesInSameComponent(StartVertex, EndVertex))
                {
                    temp.addEdge(StartVertex, EndVertex, currentEdge.Costs);
                    T.addEdge(StartVertex, EndVertex, currentEdge.Costs);
                  //  temp.deleteEdge(currentEdge);
                }
                //else
                //{
                //    temp.deleteEdge(currentEdge);
                //}

                //temp.addEdge(currentEdge.StartVertex, currentEdge.EndVertex, currentEdge.Costs);
            }

            return T;
        }

    }
}
