using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Demo.WpfGraphApplication;
namespace NETGraph.Algorithm
{
    class Dijkstra : IGraphAlgorithm
    {
        #region IGraphAlgorithm Member
        public Boolean DrawSingleStep = false;
        public Graph performAlgorithm(Graph graph, Vertex<String> startVertex)
        {
            //Graph resultGraph = new Graph();
            // resultGraph.Vertexes = graph.Vertexes;

            // ALLGEMEINES:
            //  Knoten schon besucht über Vertex.Marked
            //  Aktuelle Entfernung vom Startknoten zum Knoten über Vertex.Costs
            //  Weg zum Vorgänger über Vorgängerattribut in Knoten

            // STARTBEDINGUNGEN:
            // Die Distanz ist zu jedem Knoten ist Unendlich (in diesem Fall sehr groß)
            foreach (Vertex<String> vertex in graph.Vertexes)
            {
                vertex.Costs = double.MaxValue;
                vertex.Neighborvertex = null;
                vertex.Marked = false;

            }

            // Der aktuelle Knoten ist der Startknoten (Das Gewicht ist zu sich selber 0)
            Vertex<String> currentVertex = null;
            startVertex.Costs = 0;
            startVertex.Neighborvertex = startVertex;

            // Wiederhole bis es N-1 Kanten gibt bzw. bis alle Knoten besucht sind
            int NumOfAllVertex = graph.Vertexes.Count();

            while (graph.Vertexes.Where(x => x.Marked == false).Count() > 0)
            {
                // setzen den unbesuchten Knoten mit der geringsten Distanz als aktuell und besucht
                //komplexität nlogn (besser wäre einfach nur günstigsten knoten suchen komplexität n)
                graph.Vertexes.Sort(delegate(Vertex<String> e1, Vertex<String> e2) { return e1.Costs.CompareTo(e2.Costs); });

                for (int i = 0; i < NumOfAllVertex; i++)
                {
                    if (graph.Vertexes.ElementAt(i).Marked != true)
                    {
                        currentVertex = graph.Vertexes.ElementAt(i);
                        currentVertex.Marked = true;
                        break;
                    }
                }


                List<Vertex<String>> neighborVertexs = currentVertex.findNeighbors(graph.DirectedEdges);

                // für alle unbesuchten Nachbarn:        
                foreach (Vertex<String> vertex in neighborVertexs)
                {
                    if (vertex.Marked == false)
                    {
                        //wenn die Eigene Distanz + das Kantengewicht geringer ist als die aktuelle Distanz des Knotens
                        Edge currentEdge = graph.findEdge(currentVertex.VertexName, vertex.VertexName);
                        double currentCosts = currentVertex.Costs + currentEdge.Costs;
                        if (currentCosts < vertex.Costs)
                        {
                            // dann setze sie
                            vertex.Costs = currentCosts;

                            // und setze dich als Vorgänger
                            vertex.Neighborvertex = currentVertex;
                        }
                    }
                }
            }

            // Alle Kanten löschen die nicht in Verwendung sind
            foreach (Edge e in graph.Edges)
            {
                // Wenn der eine Knoten an der Kante einen Neighbor besitzt der der andere Knoten der Kante ist 
                if ((e.StartVertex.Neighborvertex.VertexName != e.EndVertex.VertexName) &&
                   (e.EndVertex.Neighborvertex.VertexName != e.StartVertex.VertexName))
                {
                    e.Marked = true;
                }
            }

            for (int i = 0; i < graph.Edges.Count(); i++)
            {
                if (graph.Edges[i].Marked == true)
                {
                    graph.Edges.Remove(graph.Edges.ElementAt(i));
                    i--;
                }
            }

            return graph;
        }

        #endregion
    }
}
