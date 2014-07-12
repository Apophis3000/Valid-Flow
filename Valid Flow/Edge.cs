using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valid_Flow
{
    public class Edge : ICloneable
    {
        public int Vertex1 { get; set; }
        public int Vertex2 { get; set; }
        public int MinCap { get; set; }
        public int MaxCap { get; set; }
        public int Flow { get; set; }

        public Edge(int vertex1, int vertex2, int minCap, int maxCap)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            MinCap = minCap;
            MaxCap = maxCap;
            Flow = 0;
        }

        public int IncrementCapacity(int vertexId)
        {
            if (vertexId == Vertex1)
                return Flow;
             return MaxCap - Flow;
        }

        public void AddIncrementFlow(int vertexId, int amount)
        {
            if (vertexId == Vertex1)
                Flow -= amount;
            else if (vertexId == Vertex2)
                Flow += amount;
        }

        public int Opposite(int vertex)
        {
            if (vertex == Vertex1)
                return Vertex2;
            return Vertex1;
        }

        public object Clone()
        {
            return new Edge(Vertex1, Vertex2, MinCap, MaxCap);
        }
    }
}
