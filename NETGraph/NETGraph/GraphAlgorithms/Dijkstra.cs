using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph.Algorithm
{
    class Dijkstra : IGraphAlgorithm
    {
        #region IGraphAlgorithm Member

        public Graph performAlgorithm(Graph graph, Vertex<String> startVertex)
        {
            // Graph resultGraph = new Graph();

            // !! ----  resultGraph.Vertexes = graph.Vertexes;

            // ALLGEMEINES:
            //  Knoten schon besucht über Vertex.Marked
            //  Aktuelle Entfernung vom Startknoten zum Knoten über Vertex.Costs
            //  Weg zum Vorgänger über Kanten und Kosten die in neuen Graphen eingefügt werden ??

            // STARTBEDINGUNGEN:
            // Die Distanz ist zu jedem Knoten ist Unendlich (in diesem Fall sehr groß)
            foreach (Vertex<String> vertex in graph.Vertexes)
                {
                    //Vertex<String> newVertex = new Vertex<String>(vertex.VertexName);
                    //graph.addVertex(newVertex);
                    vertex.Costs = 1000000000;
                    //newVertex.Costs = 1000000000;
                    //vertex._marked = false;
                }

            // Der aktuelle Knoten ist der Startknoten (Das Gewicht ist zu sich selber 0)
            Vertex<String> currentVertex = null;
            startVertex.Costs = 0;
            
           
           // Wiederhole bis es N-1 Kanten gibt bzw. bis alle Knoten besucht sind
            int NumOfAllVertex = graph.Vertexes.Count();

            // KANN NICHT STIMMEN
            while (graph.Edges.Count() < NumOfAllVertex - 1)
            {
                // setzen den unbesuchten Knoten mit der geringsten Distanz als aktuell und besucht
                graph.Vertexes.Sort(delegate(Vertex<String> e1, Vertex<String> e2) { return e1.Costs.CompareTo(e2.Costs); });
                
                for(int i = 0; i < NumOfAllVertex; i++)
                {
                    if (graph.Vertexes.ElementAt(i)._marked != true)
                    {
                        currentVertex = graph.Vertexes.ElementAt(i);
                        currentVertex._marked = true;
                        break;
                    }
                }

                
                List<Vertex<String>> neighborVertexs = currentVertex.findNeighbors(false);

                // für alle unbesuchten Nachbarn:        
                foreach (Vertex<String> vertex in neighborVertexs)
                {
                    if (vertex._marked == false)
                    {
                        //wenn die Eigene Distanz + das Kantengewicht geringer ist als die aktuelle Distanz des Knotens
                        double currentCosts = currentVertex.Costs + graph.findEdge(vertex.VertexName, currentVertex.VertexName).Costs;
                        Edge currentEdge = graph.findEdge(vertex.VertexName, currentVertex.VertexName);
                        if ((currentVertex.Costs + currentEdge.Costs) < vertex.Costs)
                        {
                            // dann setze sie
                            vertex.Costs = currentCosts;

                            // falls der Knoten schon einen Vorgänger (Kante) hat
                                //resultGraph.deleteEdge(vertex.Edges.Single());

                            // und setze dich als Vorgänger
                                //resultGraph.addEdge(currentEdge);
                            vertex.Neighborvertex = currentVertex;
                        }
                    }
                }
            }

            graph.Edges = null;

            foreach(Vertex<String> vertex in graph.Vertexes)
            {
                graph.addEdge(vertex, vertex.Neighborvertex);
            }


            return graph;
        }

        #endregion
    }
}
