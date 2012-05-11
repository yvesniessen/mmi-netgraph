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

                if (!vertex._marked)
                {
                    vertex._marked = true;
                    result.Vertexes.Add(vertex);
                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    if (!neighbor._marked)
                    {
                        Schlange.Add(neighbor);
                    }
                }


            } while (Schlange.Count != 0);

            return result;
        }

        public List<Vertex<String>> findWay(Graph graph, Vertex<String> startVertex, Vertex<String> endVertex)
        {
            foreach (Vertex<String> preVertex in graph.Vertexes)
            {
                preVertex.PreVertex = null;
            }

            Graph result = new Graph();

            List<Vertex<String>> Schlange = new List<Vertex<string>>();

            Schlange.Add(startVertex);

            do
            {
                Vertex<String> vertex = Schlange.First();
                Schlange.Remove(vertex);

                if (!vertex._marked)
                {
                    result.Vertexes.Add(vertex);
                    vertex._marked = true;
                    //Wir sind am Ende angekommen und können getrost aufhören
                    if (vertex.VertexName.Equals(endVertex.VertexName))
                    {
                        break;
                    }
                 
                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    if (!neighbor._marked)
                    {
                        Schlange.Add(neighbor);
                        if (neighbor.PreVertex == null)
                        {
                            neighbor.PreVertex = vertex;
                        }
                    }
                }


            } while (Schlange.Count != 0);

            List<Vertex<String>> way = new List<Vertex<string>>();
            Vertex<String> searchedVertex = graph.findVertex(endVertex.VertexName);
            way.Add(searchedVertex);

            do
            {
                searchedVertex = graph.findVertex(searchedVertex.PreVertex.VertexName);
                way.Add(searchedVertex);
            } while (searchedVertex.PreVertex.VertexName != startVertex.VertexName);

              way.Add(startVertex);

            return way;
        }

        public bool checkIfTwoVertexesinSameComponent(Graph graph, Vertex<String> startVertex, Vertex<String> endVertex)
        {
            List<Vertex<String>> Schlange = new List<Vertex<string>>();

            Schlange.Add(startVertex);

            do
            {
                Vertex<String> vertex = Schlange.First();
                Schlange.Remove(vertex);

                if (!vertex._marked)
                {
                    vertex._marked = true;
                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    if (neighbor == endVertex)
                        return true;

                    if (!neighbor._marked)
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
