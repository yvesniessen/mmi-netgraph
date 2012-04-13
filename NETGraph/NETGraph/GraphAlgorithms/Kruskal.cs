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
            Graph resultGraph = new Graph();

            Graph resultForStartVertex = new Graph();
            Graph resultForEndVertex = new Graph();

            List<Graph> temp = graph.getConnectingComponents();
            int i = temp.Count;
            IGraphAlgorithm breathSearch = new BreathSearch();

            //Liste aller Edges des Eingangsgraphen
            List<Edge> edges = graph.Edges;
            
            //Sortiere die Liste nach den Kosten
            edges.Sort(delegate(Edge e1, Edge e2) { return e1.Costs.CompareTo(e2.Costs); });

            //Sichere die sortierte Edgelist für evtl. spätere Verbindung
            List<Edge> backupEdgeList = new List<Edge>();

            List<Vertex<String>> vertexesWithEdge = new List<Vertex<string>>();
            foreach(Vertex<String> vertex in graph.Vertexes)
            {
                if (vertex.Edges.Count > 0)
                {
                    vertexesWithEdge.Add(vertex);
                }
            }


            while(resultGraph.Edges.Count < vertexesWithEdge.Count-1)
            {
                //Hole die günstigste Kante aus der Liste und entferne sie
                Edge currentEdge = edges.First();
                edges.Remove(currentEdge);

                //Erzeuge die Vertexes NEU um sie in den neuen result Graph einzufügen
                Vertex<String> startVertexForNewEdge = new Vertex<string>(currentEdge.StartVertex.VertexName);
                Vertex<String> endVertexForNewEdge = new Vertex<string>(currentEdge.EndVertex.VertexName);

                /*
                 * Problem: die Vertexes kennen ihre Nachbarn -> Deswegen funktioniert die Breitensuche nur auf dem neuen Graph
                 * 
                 * Dazu muss man allerdings erstmal die Vertexes in dem NEUEN Grauph finden!
                 * 
                */

                resultGraph.unmarkGraph();

                Vertex<String> currentVertex = resultGraph.findVertex(startVertexForNewEdge.VertexName);

                if (currentVertex.Edges.Count > 0)
                {
                    resultForStartVertex = breathSearch.performAlgorithm(resultGraph, currentVertex);
                }
                else
                {
                    resultForStartVertex = new Graph();
                }

                /*resultGraph.unmarkGraph();

                if (resultGraph.findVertex(endVertexForNewEdge.VertexName).Edges.Count > 0)
                {
                    resultForEndVertex = breathSearch.performAlgorithm(resultGraph, resultGraph.findVertex(endVertexForNewEdge.VertexName));
                }

                /*
                 * Überlegung:
                 * 1. Wenn der Start oder Endknoten bereits in einer Komponente ist -> ok (hinzufügen)
                 * 2. Wenn kein Knoten in einer Komponente ist -> ok (hinzufügen!)
                 * 3. Wenn beide Knoten bereits in einer Komponente sind -> fail!
                 * 
                 * also: Prüfe ob die Anzahl der Vertexes im resultGraph, ausgehend von current.StartVertex UND current.EndVertex NICHT größer 1 ist
                 * 
                 */

                //if ((!(resultForStartVertex.Vertexes.Count > 1 )) //&& resultForEndVertex.Vertexes.Count > 1)))
                //{
                //    resultGraph.addEdge(startVertexForNewEdge, endVertexForNewEdge);
                //}

                if( !resultForStartVertex.Vertexes.Contains(resultGraph.findVertex(endVertexForNewEdge.VertexName)) )
                {
                    resultGraph.addEdge(startVertexForNewEdge, endVertexForNewEdge, currentEdge.Costs);
                }
                else
                {
                    //Speichere die nicht genutzen Edges für später... (siehe unten)
                    backupEdgeList.Add(currentEdge);
                }
            }

            /*
             * Problem 1: es kann sein, dass zwar alle Knoten drin sind, aber diese nicht miteinander verbunden sind -> es gibt mehrere ZKomponenten,
             * die jetzt verbunden werden müssen
             * 
             * Problem 2: Lose Kanten wurden von den normalen ConnectingComponents mitgezählt - deswegen jetzt die neue Funktion: getConnectingComponentsWithoutLooseVertexes!
             * 
             */

            /*resultGraph.unmarkGraph();
            while (resultGraph.getConnectingComponentsWithoutLooseVertexes().Count > 1)
            {
                resultGraph = connectLooseComponents(resultGraph, backupEdgeList);
                resultGraph.unmarkGraph();
            }*/
            
            return resultGraph;
        }

        private Graph connectLooseComponents(Graph T, List<Edge> backupEdgeList)
        {
            Edge currentEdge = backupEdgeList.First();
            backupEdgeList.Remove(currentEdge);

            Vertex<String> startVertexForNewEdge = T.findVertex(currentEdge.StartVertex.VertexName);
            Vertex<String> endVertexForNewEdge = T.findVertex(currentEdge.EndVertex.VertexName);

            T.addEdge(startVertexForNewEdge, endVertexForNewEdge);

            return T;
        }

        private bool checkIfSameVertexes(Graph g1, Graph g2)
        {

            if (g1.Vertexes.Count == g2.Vertexes.Count)
            {
                foreach (Vertex<String> v in g1.Vertexes)
                {
                    if (!g2.Vertexes.Contains(v))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
