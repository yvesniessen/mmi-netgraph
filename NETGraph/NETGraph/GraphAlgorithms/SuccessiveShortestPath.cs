﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETGraph.Algorithm;

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
        public Graph performAlgorithm(Graph graph, Vertex<string> startVertex)
        {
            Graph result = graph;

            // Setzen der Anfangsflüsse

            foreach(Edge e in result.Edges)
            {
                if (e.RealCosts < 0)
                {
                    e.Flow = e.Costs;
                }
                else
                {
                    e.Flow = 0;
                }
            }

            foreach (Vertex<String> v in result.Vertexes)
            {
                double sumflow = 0;

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
                v.Momentbalance = v.Balance - sumflow;
            }

                Vertex<String> s;
                Vertex<String> t = new Vertex<string>("leer");
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


                                    // Die Kanten des Weges mit neuen werten füllen
                                    //way3 = findWay.findWay(way2, way2.findVertex(s.VertexName), way2.findVertex(t.VertexName));
                                    // Wenn es einen Weg gibt nimm diesen
                                    if (way.Edges.Count() >= 1 )
                                    {
                                        break;
                                    }
                                }
                            }

                            if (way.Edges.Count() >= 1)
                            {
                                break;
                            }
                        }
                    
                    }

                    if (way.Edges.Count() >= 1)
                    {
                        double landa = 0;
                        // finde Landa heraus (das min der Kapazitäten aller Kanten im Weg und dem was eine Quelle geben kann (b-b') und was eine Senke aufnehmen kann (b'-b))

                        // Sortiere alle kosten nach ihrer Kapazität
                        way.Edges.Sort(delegate(Edge e1, Edge e2) { return e1.RealCosts.CompareTo(e2.RealCosts); });
                        landa = way.Edges.First().RealCosts;

                        if (way.Vertexes.First().Momentbalance < landa)
                        {
                            landa = way.Vertexes.First().Momentbalance;
                        }

                        if (way.Vertexes.Last().Momentbalance < landa)
                        {
                            landa = way.Vertexes.Last().Momentbalance;
                        }
                        // Erhöhe alle Flüsse auf dem Weg des Residualgraphen im Ursprungsgraphen und Landa
                        // Zum Beispiel: Kante aus Residualgraph mit -3 wird im ursprünglichen Graphen zu einem Fluss von 3.

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
