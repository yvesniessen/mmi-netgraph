using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NETGraph
{
    class Export 
    {
        public static GraphList showGraph( ref Graph graph)
        {

            List<String> _edges = showEdges(ref graph);
            List<String> _vertexes = showVertexes(ref graph);
            return new GraphList(_edges, _vertexes);
        }

        public static List<String> showEdges(ref Graph graph)
        {
            List<String> _output = new List<string>();
            foreach (Edge edge in graph.getEdges())
            {
                _output.Add(edge.ToString());
            }
            return _output;
        }

        //ToDO: Change Generics to template!
        public static List<String> showVertexes(ref Graph graph)
        {
            List<String> _output = new List<string>();
            foreach (Vertex<String> vertex in graph.getVertexes())
            {
                _output.Add(vertex.ToString());
            }
            return _output;
        }

        public static void printGraph(GraphList graphlist)
        {
            Debug.Print("Vertexes");
            foreach(String vertex in graphlist.Vertexes)
            {
                Debug.Print(vertex);
            }
            Debug.Print("Edges");
            foreach (String edges in graphlist.Edges)
            {
                Debug.Print(edges);
            }
        }
    }
}
