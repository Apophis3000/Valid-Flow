using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Valid_Flow
{    
    public class Program
    {
        static void Main(string[] args)
        {
            FlowNetwork n = FlowNetwork.CreateNetworkFromFile("Data.txt");
            n.Source = 16;
            n.Sink = 22;
            
            FlowNetwork nh = n.GetExpandedNetwork();
            MaxFlow mf = new MaxFlow(nh);

            n.FindAllowedFlow(mf.FlowEdges);
            n.PrintFlowNetwork();
        }
    }
}