using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;

namespace NETGraph.GraphAlgorithms
{
    class Prim : IGraphAlgorithm
    {
        #region IGraphAlgorithm Member

        /*
         * Vorgehen - Algorithmus von Prim:

                1. Ausgangspunkt für das Verfahren ist ein beliebiger Startknoten
                2. alle Kanten zu Nachbarknoten werden in eine Nachbarliste eingefügt, man wählt eine Kante minimaler Länge aus der Nachbarliste und fügt diese Kante dem bereits initialisierten Spannbaum zu
                3. von dort wird wieder der minimale Weg, basierend auf der ausgewählten Kante, zum nächsten Knoten gewählt, ist dieser Knoten bereits besucht worden, wird er nicht berücksichtigt
                4. dieses Verfahren führt man durch, bis alle Knoten besucht wurden
         * 
        */

        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Graph T = new Graph();

            //Startvertex wird nicht betrachtet
            startVertex.Marked = true;
            List<Vertex<String>> unmarkedVertexes = getUnmarkedVertexes(graph.Vertexes);
            List<Edge> nachbarListe = new List<Edge>();

            T.addVertex(startVertex);
            
            //Gehe solange über die Liste bis sie leer ist
            while (unmarkedVertexes.Count > 0)
            {
                //Hole alle unbesuchten Kanten, von den Knoten die bereits in T sind
                //Marked = schon in T

                nachbarListe.Clear();

                List<Vertex<String>> markedVertexes = getMarkedVertexes(graph.Vertexes);

                foreach (Edge edge in graph.Edges)
                {
                    //Die Egde darf nicht markiert (besucht) sein UND muss entweder den Start oder Endpunkt in T haben, damit sie in Frage kommt
                    if (!edge.Marked && (markedVertexes.Contains(edge.StartVertex) || markedVertexes.Contains(edge.EndVertex)))
                    {
                        if (!edge.StartVertex.Marked || !edge.EndVertex.Marked)
                        {
                            nachbarListe.Add(edge);
                        }
                    }
                }

                Edge cheapestEdge = getCheapestEdge(nachbarListe);

                //Setze (zur Sicherheit) beide Knoten und die Kante auf markiert
                cheapestEdge.StartVertex.Marked = true;
                cheapestEdge.EndVertex.Marked = true;
                cheapestEdge.Marked = true;

                //Füge die Kante in T ein (und somit auch den Knoten)
                T.addEdge(cheapestEdge.StartVertex, cheapestEdge.EndVertex, cheapestEdge.Costs);

                unmarkedVertexes = getUnmarkedVertexes(graph.Vertexes);
            } 

            return T;
        }

        #endregion

        #region Private Methoden

        private Edge getCheapestEdge(List<Edge> edges)
        {
            Edge tempEdge = edges.First();
            foreach (Edge e in edges)
            {
                if (e.Costs < tempEdge.Costs)
                {
                    tempEdge = e;
                }
            }
            return tempEdge;
        }

        private List<Vertex<String>> getMarkedVertexes(List<Vertex<String>> vertexes)
        {
            var vertexList = from vertex in vertexes
                             where (vertex.Marked) && (vertex.Edges.Count > 0)
                             select vertex;

            return vertexList.ToList(); ;
        }

        private List<Vertex<String>> getUnmarkedVertexes(List<Vertex<String>> vertexes)
        {
            var vertexList = from vertex in vertexes
                             where (!vertex.Marked) && (vertex.Edges.Count > 0)
                             select vertex;

            return vertexList.ToList(); ;
        }

        private List<Edge> getUnmarkedEdges(List<Edge> edges)
        {
            var edgeList = from edge in edges
                           where !edge.Marked
                           select edge;

            return edgeList.ToList();
        }

        #endregion
    }
}
