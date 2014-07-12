using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Valid_Flow
{
    public class FlowNetwork
    {
        public List<Edge> Edges { get; set; }
        public List<Vertex> Vertices { get; set; }
        public int Source { get; set; }
        public int Sink { get; set; }
        private const int INFINITY = int.MaxValue;

        public FlowNetwork(List<Edge> edges, List<Vertex> vertices)
        {
            Edges = edges;
            Vertices = vertices;
        }

        private string GetVertexName(int id)
        {
            if (id == Source)
                return "q";
            else if (id == Sink)
                return "s";
            else
                return id.ToString();
        }

        public static FlowNetwork CreateNetworkFromFile(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            List<Edge> edges = new List<Edge>();
            List<Vertex> vertices = new List<Vertex>();

            foreach (var str in lines)
            {
                string[] edgeString = str.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int[] edgeInt = new int[edgeString.Length];
                for (int i = 0; i < edgeInt.Length; i++)
                {
                    edgeInt[i] = int.Parse(edgeString[i]);
                }

                if (!vertices.Contains(vertices.Find(x => x.Id == edgeInt[0])))
                {
                    vertices.Add(new Vertex(edgeInt[0]));
                }

                if (!vertices.Contains(vertices.Find(x => x.Id == edgeInt[1])))
                {
                    vertices.Add(new Vertex(edgeInt[1]));
                }
                edges.Add(new Edge(edgeInt[0], edgeInt[1], edgeInt[2], edgeInt[3]));
            }

            return new FlowNetwork(edges, vertices);
        }
        
        public void PrintFlowNetwork()
        {
            Console.WriteLine("Allowed flow:");
            Console.WriteLine("(q - source, s - sink)\n");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("Node 1   | Node 2   | Min-Cap.      | Max-Cap.      | Flow");
            Console.WriteLine("--------------------------------------------------------------------");
            foreach (Edge e in Edges)
            {
                Console.WriteLine("   {0,-5} |    {1,-5} |       {2,-7} |    {3, -10} |   {4, -5}", GetVertexName(e.Vertex1), GetVertexName(e.Vertex2),
                    e.MinCap, e.MaxCap == INFINITY ? "Inf." : e.MaxCap.ToString(), e.Flow);
            }
            Console.WriteLine("--------------------------------------------------------------------");
        }

        public FlowNetwork GetExpandedNetwork()
        {
            FlowNetwork g = new FlowNetwork(Edges.Select(item => (Edge)item.Clone()).ToList(), this.Vertices.Select(item => (Vertex)item.Clone()).ToList());
            Dictionary<int, int> indiceTable = new Dictionary<int, int>();

            foreach (Vertex v in Vertices)
            {
                int value = 0;

                foreach (Edge e in g.Edges.Where(x => x.Vertex2 == v.Id))
                {
                    value += e.MinCap;
                }
                foreach (Edge e in g.Edges.Where(x => x.Vertex1 == v.Id))
                {
                    value -= e.MinCap;
                }

                indiceTable.Add(v.Id, value);
            }

            foreach (Edge e in g.Edges.Where(x => x.MinCap > 0))
            {
                e.MaxCap = e.MaxCap - e.MinCap;
                e.MinCap = 0;
            }

            g.Source = 0;
            g.Sink = g.Vertices.Count() + 1;
            g.Vertices.Add(new Vertex(g.Source));
            g.Vertices.Add(new Vertex(g.Sink));

            foreach (var p in indiceTable.Where(x => x.Value != 0))
            {
                if (p.Value > 0)
                {
                    g.Edges.Add(new Edge(g.Source, p.Key, 0, p.Value));
                }
                else if (p.Value < 0)
                {
                    g.Edges.Add(new Edge(p.Key, g.Sink, 0, Math.Abs(p.Value)));
                }
            }
            g.Edges.Add(new Edge(Sink, Source, 0, INFINITY));
            return g;
        }

        public void FindAllowedFlow(List<Edge> flowEdges)
        {
            foreach (Edge e in Edges)
            {
                e.Flow += e.MinCap;

                foreach (Edge fe in flowEdges.Where(x => x.Vertex1 == e.Vertex1 && x.Vertex2 == e.Vertex2))
                {
                    e.Flow += fe.Flow;
                }
            }
        }        
    }
}
