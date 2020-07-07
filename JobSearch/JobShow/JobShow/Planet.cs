using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataGridColumnsDemo
{
    public class Planet
    {
        public string Name { get; set; }
        public int Moons { get; set; }
        public bool HasRings { get; set; }
        public bool? GlobalMagneticField { get; set; }
        public string Category { get; set; }
        public Uri MoreInfoUri { get; set; }
    }
}
