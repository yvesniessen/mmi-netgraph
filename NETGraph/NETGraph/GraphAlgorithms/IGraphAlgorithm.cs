using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NETGraph.Algorithm
{
    interface IGraphAlgorithm
    {
       Graph performAlgorithm(Graph graph, Vertex<String> startVertex);
    }
}
