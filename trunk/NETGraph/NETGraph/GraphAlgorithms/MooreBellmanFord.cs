using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NETGraph.Algorithm
{
    class MooreBellmanFord : IGraphAlgorithm
    {
        #region IGraphAlgorithm Member

        /////////////////////////////////////////////////////////////////////////////////////////
        // TODO: im Schritt 3 negative Zykel auffinden...!!!!
        // Siehe Algorithmusbeschreibung
        /////////////////////////////////////////////////////////////////////////////////////////

       public Graph performAlgorithm(Graph graph, Vertex<String> startVertex)
        {
           graph.unmarkGraph();
           int n = graph.Vertexes.Count;

           /////////////////////////////////////////////////////////////////////////////////////////
           // Schritt 1: INIT, siehe Algorithmusvorschrift
           /////////////////////////////////////////////////////////////////////////////////////////

           foreach (Vertex<String> vertex in graph.Vertexes)
           {
               vertex.Costs = double.PositiveInfinity;
               vertex.PreVertex = null;
           }

           startVertex.Costs = 0;
           startVertex.PreVertex = startVertex;
           startVertex.Marked = true;


           /////////////////////////////////////////////////////////////////////////////////////////
           // Schritt 2: Die günstigsten Strecken von startVertex aus berechnen
           /////////////////////////////////////////////////////////////////////////////////////////

           //List<Vertex<String>> neighborList = new List<Vertex<string>>();
           for (int i = 1; i < n; i++)
           {
               //Hole die Nachbarn für den aktuellen Vertex und schaue sie dir an
               foreach (Edge edge in graph.Edges)
               //foreach (Vertex<String> vertex in currentVertex.findNeighbors(graph.DirectedEdges))
               {
                   //if (!vertex.Marked)
                   //{
                   //    vertex.Marked = true;
                   //    neighborList.Add(vertex);
                   //}

                   //double distanceFromCurrentVertex = currentVertex.Costs;
                   double edgeCostsFromCurrentEdge = edge.Costs;

                   if ((edge.StartVertex.Costs + edgeCostsFromCurrentEdge) < edge.EndVertex.Costs)
                   {
                       edge.EndVertex.Costs = (edge.StartVertex.Costs + edgeCostsFromCurrentEdge);
                       edge.EndVertex.PreVertex = edge.StartVertex;
                   }
               }
           }



           /////////////////////////////////////////////////////////////////////////////////////////
           // Schritt 3: Die benutzen Kanten raussuchen und prüfen, ob es einen negativen Zykel gibt
           /////////////////////////////////////////////////////////////////////////////////////////

           foreach (Edge edge in graph.Edges)
           {
               double startVertexCosts = edge.StartVertex.Costs;
               double endVertexCosts = edge.EndVertex.Costs;
               if ((startVertexCosts + edge.Costs) < endVertexCosts)
                   EventManagement.GuiLog("Abbruch: Es gibt einen Kreis negativen Gewichtes.");

           //TODO: hier die KANTE zurückliefen für nächstes Praktikum 
           }

           return graph;
        }

        #endregion
    }
}
