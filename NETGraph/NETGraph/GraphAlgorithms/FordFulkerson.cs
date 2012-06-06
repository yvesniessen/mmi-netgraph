using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;
using Demo.WpfGraphApplication;

namespace NETGraph.GraphAlgorithms
{
    class FordFulkerson : IGraphAlgorithm
    {
       // public Vertex<string> EndVertex;
        public Boolean DrawSingleStep = true;
        public Vertex<String> EndVertex = null;




        private int counter = 0;
        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            if (EndVertex == null)
            {
                EndVertex = graph.Vertexes.Last();
            }
            Graph minimalerWeg = new Graph();
            Graph residualGraph = new Graph();
            
            double min_costs = 0;

            // Solange wie es einen Fluss noch von startVertex nach endVertex gibt ist die Optimierung noch nicht beendet
            residualGraph = graph;
            residualGraph.DirectedEdges = true;
            minimalerWeg = residualGraph.getWayForVertexes(startVertex, EndVertex);
            if (minimalerWeg.Vertexes.Count == 0)
            EventManagement.GuiLog("Keinen initialen Weg von "+startVertex.ToString()+" zu "+EndVertex.ToString()+" gefunden.");
            while (minimalerWeg.Vertexes.Count != 0)
            {

                residualGraph.unmarkGraph();
                graph.unmarkGraph();

                // Suche die maximalen Fluss des gefundenen Weges im ResidualGraphen
                min_costs = getMinCostsFromEdges(minimalerWeg);

                // Suche alle Kanten im Residualgraphen und passe die Kapazitäten im ursprünglichen Graphen an
                foreach (Edge edge in minimalerWeg.Edges)
                {
                    Edge currentedge = graph.findEdge(edge);
                    if (currentedge != null)
                    {
                        currentedge.Flow += min_costs;
                    }
                    else
                    {
                        currentedge = graph.findInvertedEdge(edge);
                        if (currentedge != null)
                            currentedge.Flow -= min_costs;
                    }
                }

                // Wende die Breitensuche auf dem Residualgraphen an und finde den MINIMALEN(Kantenanzahl) Weg
                residualGraph = this.buildResidualGraph(graph);
                minimalerWeg = residualGraph.getWayForVertexes(residualGraph.findVertex(startVertex.VertexName), residualGraph.findVertex(EndVertex.VertexName));
               
                if (DrawSingleStep)
                {
                    counter++;
                    Window1 graphdraw = new Window1(graph);
                    graphdraw.Title = "FordFulkerson Durchlauf " + counter.ToString() + ": Graph";
                    graphdraw.Show();

                    Window1 residualGraphdraw = new Window1(residualGraph);
                    residualGraphdraw.Title = "FordFulkerson Durchlauf " + counter.ToString() + ": residualGraph";
                    residualGraphdraw.Show();

                    Window1 minimalerWegDraw = new Window1(minimalerWeg);
                    minimalerWegDraw.Title = "FordFulkerson Durchlauf " + counter.ToString() + ": minimalerWeg";
                    minimalerWegDraw.Show();
                }
            
            }
            return graph;
        }

        public Graph buildResidualGraph(Graph graph)
        {
            Graph residualGraph = new Graph();
            residualGraph.DirectedEdges = true;
            foreach (Edge e in graph.Edges)
            {
                Vertex<String> startVertexForNewEdge = new Vertex<string>(e.StartVertex.VertexName);
                Vertex<String> endVertexForNewEdge = new Vertex<string>(e.EndVertex.VertexName);


                //Hinkante hinzufügen
                if ((e.Costs - e.Flow) > 0)
                {
                    residualGraph.addEdge(startVertexForNewEdge, endVertexForNewEdge, (e.Costs - e.Flow), e.RealCosts);
                }

                //Rückkante hinzufügen
                if (e.Flow != 0)
                {
                    //if (e.RealCosts >= 0)
                        residualGraph.addEdge(endVertexForNewEdge, startVertexForNewEdge, e.Flow, (e.RealCosts * (-1)));
                   // else
                   //     residualGraph.addEdge(endVertexForNewEdge, startVertexForNewEdge, e.Costs, e.RealCosts);
                }
            }
            return residualGraph;
        }

        public double getMinCostsFromEdges(Graph minWeg)
        {
            double costs = 0.0;
            minWeg.Edges.Sort(delegate(Edge e1, Edge e2) { return e1.Costs.CompareTo(e2.Costs); });
            costs = minWeg.Edges.First().Costs;
            return costs;
        }
    }
}
