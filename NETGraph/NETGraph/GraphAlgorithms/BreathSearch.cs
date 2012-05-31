using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph.Algorithm
{
    class BreathSearch : IGraphAlgorithm
    {
        #region IGraphAlgorithm Member

        public Graph performAlgorithm(Graph graph, Vertex<String> startVertex)
        {
            Graph result = new Graph();

            List<Vertex<String>> Schlange = new List<Vertex<string>>();

            Schlange.Add(startVertex);

            do
            {
                Vertex<String> vertex = Schlange.First();
                Schlange.Remove(vertex);

                if (!vertex.Marked)
                {
                    vertex.Marked = true;
                    result.Vertexes.Add(vertex);
                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    if (!neighbor.Marked)
                    {
                        Schlange.Add(neighbor);
                    }
                }


            } while (Schlange.Count != 0);

            return result;
        }

        public Graph findWay(Graph graph, Vertex<String> startVertex, Vertex<String> endVertex)
        {
            foreach (Vertex<String> preVertex in graph.Vertexes)
            {
                preVertex.PreVertex = null;
            }

            Graph result = new Graph();

            List<Vertex<String>> Schlange = new List<Vertex<string>>();

            Schlange.Add(startVertex);
            Vertex<String> vertex = Schlange.First();

            // Solange nicht beim letzten Element angekommen
            while (!Schlange.Contains(endVertex) && (Schlange.Count != 0))
            {
                Schlange.Remove(vertex);

                if (!vertex.Marked)
                {
                    //result.Vertexes.Add(vertex);
                    vertex.Marked = true;
                    //Wir sind am Ende angekommen und können getrost aufhören
                    //if (vertex.VertexName.Equals(endVertex.VertexName))
                    //{
                    //    break;
                    //}

                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    //if (!neighbor.Marked)
                    //{
                    //    Schlange.Add(neighbor);
                    //    if (neighbor.PreVertex == null)
                    //    {
                    //        neighbor.PreVertex = vertex;
                    //    }
                    //}

                    // Wenn noch kein Vorgänger für den Knoten eingetragen ist, sprich er von noch keinem anderen Knoten in die Liste eingetragen wurde
                    if (neighbor.PreVertex == null)
                    {
                        Schlange.Add(neighbor);
                        neighbor.PreVertex = vertex;
                    }
                }
                if (Schlange.Count > 0)
                {
                    vertex = Schlange.First();
                }

            }

            //solange aktueller Knoten != Startknoten

            if (endVertex.PreVertex != null)
            {
                Vertex<String> Vertex = endVertex;
                //Solange bis wir am Anfang des Weges sind
                while (Vertex.VertexName != startVertex.VertexName)
                {
                    result.addEdge(Vertex.PreVertex, Vertex, graph.findEdge(Vertex.PreVertex, Vertex).Costs, graph.findEdge(Vertex.PreVertex, Vertex).RealCosts);
                    Vertex = Vertex.PreVertex;
                }
            }

            return result;


            //List<Vertex<String>> way = new List<Vertex<string>>();
            //Vertex<String> searchedVertex = graph.findVertex(endVertex.VertexName);
            //way.Add(searchedVertex);

            //do
            //{
            //    searchedVertex = graph.findVertex(searchedVertex.PreVertex.VertexName);
            //    way.Add(searchedVertex);
            //} while (searchedVertex.PreVertex.VertexName != startVertex.VertexName);

            //  way.Add(startVertex);

            //return way;
        }

        public Graph findCircle(Graph graph, Vertex<String> startVertex, Vertex<String> endVertex)
        {
            foreach (Vertex<String> preVertex in graph.Vertexes)
            {
                preVertex.PreVertex = null;
            }

            Graph result = new Graph();

            List<Vertex<String>> Schlange = new List<Vertex<string>>();

            Schlange.Add(startVertex);
            Vertex<String> vertex = Schlange.First();

            // Solange nicht beim letzten Element angekommen
            while ((Schlange.Count != 0))
            {
                Schlange.Remove(vertex);

                if (!vertex.Marked)
                {
                    //result.Vertexes.Add(vertex);
                    vertex.Marked = true;
                    //Wir sind am Ende angekommen und können getrost aufhören
                    //if (vertex.VertexName.Equals(endVertex.VertexName))
                    //{
                    //    break;
                    //}

                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    //if (!neighbor.Marked)
                    //{
                    //    Schlange.Add(neighbor);
                    //    if (neighbor.PreVertex == null)
                    //    {
                    //        neighbor.PreVertex = vertex;
                    //    }
                    //}

                    // Wenn noch kein Vorgänger für den Knoten eingetragen ist, sprich er von noch keinem anderen Knoten in die Liste eingetragen wurde
                    if (neighbor.PreVertex == null)
                    {
                        Schlange.Add(neighbor);
                        neighbor.PreVertex = vertex;
                    }
                }
                if (Schlange.Count > 0)
                {
                    vertex = Schlange.First();
                }

            }

            //solange aktueller Knoten != Startknoten

            if (endVertex.PreVertex != null)
            {
                Vertex<String> Vertex = endVertex;
                //Solange bis wir am Anfang des Weges sind
                while (Vertex.VertexName != startVertex.VertexName)
                {
                    result.addEdge(Vertex.PreVertex, Vertex, graph.findEdge(Vertex.PreVertex, Vertex).Costs);
                    Vertex = Vertex.PreVertex;
                }
            }

            return result;
        }

        public bool checkIfTwoVertexesinSameComponent(Graph graph, Vertex<String> startVertex, Vertex<String> endVertex)
        {
            List<Vertex<String>> Schlange = new List<Vertex<string>>();

            Schlange.Add(startVertex);

            do
            {
                Vertex<String> vertex = Schlange.First();
                Schlange.Remove(vertex);

                if (!vertex.Marked)
                {
                    vertex.Marked = true;
                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    if (neighbor == endVertex)
                        return true;

                    if (!neighbor.Marked)
                    {
                        Schlange.Add(neighbor);
                    }
                }


            } while (Schlange.Count != 0);

            return false;
        }

        #endregion
    }
}
