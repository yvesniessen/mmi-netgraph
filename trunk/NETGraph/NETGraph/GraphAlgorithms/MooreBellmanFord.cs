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
           Graph resultGraph = new Graph();
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
           Vertex<String> currentVertex = startVertex;



           /////////////////////////////////////////////////////////////////////////////////////////
           // Schritt 2: Die günstigsten Strecken von startVertex aus berechnen
           /////////////////////////////////////////////////////////////////////////////////////////

           List<Vertex<String>> neighborList = new List<Vertex<string>>();
           for (int i = 0; i < n; i++)
           {
               //Hole die Nachbarn für den aktuellen Vertex und schaue sie dir an
               foreach (Vertex<String> vertex in currentVertex.findNeighbors(graph.DirectedEdges))
               {
                   if (!vertex.Marked)
                   {
                       vertex.Marked = true;
                       neighborList.Add(vertex);
                   }

                   double distanceFromCurrentVertex = currentVertex.Costs;
                   double edgeCostsFromCurrentEdge = graph.findEdge(currentVertex.VertexName, vertex.VertexName).Costs;

                   if ((distanceFromCurrentVertex + edgeCostsFromCurrentEdge) < vertex.Costs)
                   {
                       vertex.Costs = (distanceFromCurrentVertex + edgeCostsFromCurrentEdge);
                       vertex.PreVertex = currentVertex;
                   }
               }

               if (neighborList.Count > 0)
               {
                   currentVertex = neighborList.First();
                   neighborList.Remove(neighborList.First());
               }
               else
               {
                   break;
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
           }

           return graph;
        }
        #endregion
    }
}
