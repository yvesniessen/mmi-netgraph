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

        public List<Vertex<String>> findWay(Graph graph, Vertex<String> startVertex, Vertex<String> endVertex)
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

                    //Wir sind am Ende angekommen und können getrost aufhören
                    if (vertex.VertexName.Equals(endVertex.VertexName))
                        break;
                }

                List<Vertex<String>> neighbors = vertex.findNeighbors(graph.DirectedEdges);

                foreach (Vertex<String> neighbor in neighbors)
                {
                    if (!neighbor.Marked)
                    {
                        neighbor.PreVertex = vertex;
                        Schlange.Add(neighbor);
                    }
                }


            } while (Schlange.Count != 0);

            List<Vertex<String>> way = new List<Vertex<string>>();
            Vertex<String> searchedVertex = graph.findVertex(endVertex.VertexName);

            do
            {
                way.Add(searchedVertex);
                searchedVertex = graph.findVertex(searchedVertex.PreVertex.VertexName);
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
