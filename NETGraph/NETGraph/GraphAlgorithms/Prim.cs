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

        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Graph T = new Graph();
            /*Führe eine Breitensuche auf dem graphen aus um alle Knoten zu erhalten
            IGraphAlgorithm tempAlgo = new BreathSearch();
            Graph extracedGraph = tempAlgo.performAlgorithm(graph, startVertex);

            //Ergebnisgraph
           
               /* 
            extracedGraph.Edges.Clear(); //Breitensuche liefert nicht alle Kanten!

            //Tüte alle Edges in den extrahierten Graphen
            foreach (Vertex<String> vertex in graph.Vertexes)
            {
                foreach (Edge e in vertex.Edges)
                {
                    extracedGraph.addEdge(e.StartVertex, e.EndVertex); 
                }
            }

            extracedGraph.unmarkGraph();
            

            /*Vorgehen - Algorithmus von Prim:

                1. Ausgangspunkt für das Verfahren ist ein beliebiger Startknoten
                2. alle Kanten zu Nachbarknoten werden in eine Nachbarliste eingefügt, man wählt eine Kante minimaler Länge aus der Nachbarliste und fügt diese Kante dem bereits initialisierten Spannbaum zu
                3. von dort wird wieder der minimale Weg, basierend auf der ausgewählten Kante, zum nächsten Knoten gewählt, ist dieser Knoten bereits besucht worden, wird er nicht berücksichtigt
                4. dieses Verfahren führt man durch, bis alle Knoten besucht wurden
             */

            //Startvertex wird nicht betrachtet
            startVertex.Marked = true;
            List<Vertex<String>> unmarkedVertexes = getUnmarkedVertexes(graph.Vertexes);
            List<Edge> nachbarListe = new List<Edge>();

            T.addVertex(startVertex);
            
            //Gehe solange über die Liste bis sie leer ist
            do
            {
                unmarkedVertexes = getUnmarkedVertexes(graph.Vertexes);

                //Hole alle unbesuchten Kanten, von den Knoten die bereits in T sind
                //Marked = schon in T

                nachbarListe.Clear();

                List<Vertex<String>> markedVertexes = getMarkedVertexes(graph.Vertexes);
                foreach (Vertex<String> vertex in markedVertexes)
                {
                    foreach (Edge edge in vertex.Edges)
                    {
                        if (!edge.Marked)
                        {
                            nachbarListe.Add(edge);
                        }
                    }
                }

                //Hole aus der Nachbarliste die günstigste Kante
                //Min() vergleicht die Kosten der Edges!
                Edge cheapestEdge = nachbarListe.Min();

                //Setze (zur Sicherheit) beide Knoten und die Kante auf markiert
                cheapestEdge.StartVertex.Marked = true;
                cheapestEdge.EndVertex.Marked = true;
                cheapestEdge.Marked = true;

                //Füge die Kante in T ein (und somit auch den Knoten)
                T.addEdge(cheapestEdge.StartVertex, cheapestEdge.EndVertex);  
                

            } while (unmarkedVertexes.Count != 0);

            return T;
        }

        private List<Vertex<String>> getMarkedVertexes(List<Vertex<String>> vertexes)
        {
            var vertexList = from vertex in vertexes
                             where vertex.Marked
                             select vertex;

            return vertexList.ToList(); ;
        }

        private List<Vertex<String>> getUnmarkedVertexes(List<Vertex<String>> vertexes)
        {
            var vertexList = from vertex in vertexes
                             where !vertex.Marked
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
