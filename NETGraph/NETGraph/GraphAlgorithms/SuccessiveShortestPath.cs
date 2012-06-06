using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;
using Demo.WpfGraphApplication;
namespace NETGraph.GraphAlgorithms
{
    class SuccessiveShortestPath : IGraphAlgorithm
    {

        /*
        Imput:
         Graph G mit:
                Kanten:
                    - u(e) ->  Costs ???
                    - c(e) ->  Kapazitäten (Realcosts) ???
                    - f(e) ->  Fluss (am Anfang 0 bzw. bei neg. Capazitäten in Höhe der Kosten)
         
                Knoten:
                    - b(v) -> Balance (Balance)
                    - b'(v)-> (Momentbalance) = Balance - (Diffenz der Eingehenden und ausgehenden Flüsse) 



        */
        public Boolean DrawSingleStep = false;
        private Graph setBalance(Graph result)
        {
            double totalBalance = 0;
            foreach (Vertex<String> v in result.Vertexes)
            {
                double sumflow = 0;

                // Flüsse betrachten die von Knoten weg- bzw. hingehen und diese von der Balance abziehen
                foreach (Edge e in result.Edges)
                {
                    if (e.StartVertex == v)
                    {
                        sumflow += e.Flow;
                    }
                    else if (e.EndVertex == v)
                    {
                        sumflow -= e.Flow;
                    }
                }
                totalBalance += v.Balance;
                v.Momentbalance = v.Balance - sumflow;
            }
            if (totalBalance != 0)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        private Graph buildWay(Graph graph, Vertex<String> sourceVertex, Vertex<String> targetVertex)
        {
            Graph result = new Graph();
            Vertex<String> target = graph.findVertex(targetVertex.VertexName);
            Vertex<String> currentVertex = graph.findVertex(targetVertex.VertexName).PreVertex;
            Edge tempEdge;

            while (!currentVertex.VertexName.Equals(sourceVertex.VertexName))
            {
                tempEdge = graph.findEdge(currentVertex, target);
                result.addEdge(new Vertex<string>(currentVertex.VertexName), new Vertex<string>(target.VertexName),tempEdge.Costs, tempEdge.RealCosts);
                target = currentVertex;
                currentVertex = target.PreVertex;
            }

            tempEdge = graph.findEdge(currentVertex, target);
            result.addEdge(new Vertex<string>(currentVertex.VertexName), new Vertex<string>(target.VertexName), tempEdge.Costs, tempEdge.RealCosts);

            return result;
        }

        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Graph result = graph;

            // Setzen der Anfangsflüsse

            foreach(Edge e in result.Edges)
            {
                //wenn negative Kosten wird fluss voll ausgenutzt
                if (e.RealCosts < 0)
                {
                    e.Flow = e.Costs;
                }
                else
                {
                    //wenn positiv wird sie nicht genutzt
                    e.Flow = 0;
                }
            }



            result = this.setBalance(result);
            if (result == null)
            {
                EventManagement.GuiLog("KALTE FUSION, ENERGIE QUELLE AUS NICHTS - ABBRUCH ABBRUCH!!");
                return new Graph();
            }


                Vertex<String> s = new Vertex<string>("leer"); ;
                Vertex<String> t = new Vertex<string>("leer2");
                Graph way = new Graph();
                Graph way2 = new Graph();
                Graph way3 = new Graph();

                MooreBellmanFord_tmp shortestWay= new MooreBellmanFord_tmp();
                FordFulkerson residualgraph = new FordFulkerson();
                BreathSearch findWay = new BreathSearch();
                bool abort = false;

                // Solange wie es kein return gab und noch Wege von s->t gibt.
                while (!abort)
                {
                    
                    foreach (Vertex<String> v1 in result.Vertexes)
                    {
                        // Finde eine Quelle s
                        if (v1.Momentbalance > 0)
                        {
                            // Mache sie zur aktuellen Quelle
                            s = v1;

                            foreach (Vertex<String> v2 in result.Vertexes)
                            {
                                // Finde eine Senke
                                if (v2.Momentbalance < 0)
                                {
                                    // Mache sie zur aktuellen Senke
                                    t = v2;

                                    // Finde die kürzesten Wege von s aus.
                                    way = residualgraph.buildResidualGraph(result);
                                    way2 = shortestWay.performAlgorithm(way, way.findVertex(s.VertexName));
                                    way3 = this.buildWay(way2, way2.findVertex(s.VertexName), way2.findVertex(t.VertexName));

                                    // Die Kanten des Weges mit neuen werten füllen
                                    // Wenn es einen Weg gibt nimm diesen
              
                                }
                            }
                        }
                    
                    }

                    if (way3.Edges.Count() >= 1)
                    {
                        double lamda = 0;
                        // finde Landa heraus (das min der Kapazitäten aller Kanten im Weg und dem was eine Quelle geben kann (b-b') und was eine Senke aufnehmen kann (b'-b))

                        // Sortiere alle kosten nach ihrer Kapazität
                        way3.Edges.Sort(delegate(Edge e1, Edge e2) { return e1.Costs.CompareTo(e2.Costs); });
                        lamda = way3.Edges.First().Costs;

                        if (s.Momentbalance < lamda)
                        {
                            lamda = s.Momentbalance;
                        }

                        if ((t.Momentbalance*(-1)) < lamda)
                        {
                            lamda = t.Momentbalance*(-1);
                        }

                        // Erhöhe alle Flüsse auf dem Weg des Residualgraphen im Ursprungsgraphen und Landa
                        // Zum Beispiel: Kante aus Residualgraph mit -3 wird im ursprünglichen Graphen zu einem Fluss von 3.
                        foreach (Edge e in way3.Edges)
                        {
                            Edge resultedge = result.findEdge(e.StartVertex, e.EndVertex);
                            if (resultedge != null)
                            {
                                resultedge.Flow += lamda;
                            }
                            else  // Falls zurückgepumt werden muss
                            {
                                resultedge = result.findEdge(e.EndVertex, e.StartVertex);

                                if (resultedge != null)
                                {
                                    resultedge.Flow -= lamda;
                                }
                            }

                        }
                        way3 = new Graph();

                        result = this.setBalance(result);

                    }
                    else
                    {
                        int Counter = 0;
                        foreach (Vertex<String> v in result.Vertexes)
                        {
                            if (v.Momentbalance == 0)
                            {
                                Counter++;
                            }
                        }

                        if (Counter == result.Vertexes.Count())
                        {
                            return result;
                        }
                        else
                        {
                            abort = true;
                        }
                    }

                }

                return new Graph();
            }
            
        }

    }
