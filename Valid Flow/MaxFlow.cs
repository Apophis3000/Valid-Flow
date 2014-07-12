using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valid_Flow
{
    public class MaxFlow
    {
        private Dictionary<int, Edge> flowEdges;
        public int FlowValue { get; private set; }

        public List<Edge> FlowEdges
        {
            get { return flowEdges.Values.ToList(); }
        }

        public MaxFlow(FlowNetwork g)
        {
            FindMaxFlow(g);
        }

        private void FindMaxFlow(FlowNetwork g)
        {
            flowEdges = new Dictionary<int, Edge>();

            FlowValue = 0;
            int delta = int.MaxValue;
            while (HasAugmentingPath(g))
            {
                for (int v = g.Sink; v != g.Source; v = flowEdges[v].Opposite(v))
                {
                    delta = Math.Min(delta, flowEdges[v].IncrementCapacity(v));
                }
                for (int v = g.Sink; v != g.Source; v = flowEdges[v].Opposite(v))
                {
                    flowEdges[v].AddIncrementFlow(v, delta);
                }
                FlowValue += delta;
            }
        }

        private bool HasAugmentingPath(FlowNetwork g)
        {
            bool[] marked = new bool[g.Vertices.Count];            
            Queue<int> q = new Queue<int>();

            marked[g.Source] = true;
            marked[g.Sink] = false;
            q.Enqueue(g.Source);

            while (q.Count > 0)
            {
                int v1 = q.Dequeue();

                foreach (Edge e in g.Edges.Where(x => x.Vertex1 == v1 || x.Vertex2 == v1))
                {
                    int v2 = e.Opposite(v1);

                    if (e.IncrementCapacity(v2) > 0 && !marked[v2])
                    {
                        marked[v2] = true;
                        flowEdges[v2] = e;
                        q.Enqueue(v2);
                    }

                }
            }
            return marked[g.Sink];
        }
    }
}
