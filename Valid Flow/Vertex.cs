using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Valid_Flow
{
    public class Vertex : ICloneable
    {
        public Vertex(int id)
        {
            Id = id;
        }

        public int Id
        {
            get;
            private set;
        }

        public object Clone()
        {
            return new Vertex(Id);
        }
    }
}
