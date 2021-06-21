using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Geometry : Feature
    {
        public IEnumerable<Point> Points = new List<Point>();
         
        public IEnumerable<double[]> Coordinates { get => Points.Select(x => x.Coordinates); }

        public string Label { get; set; } = "";

        public int Width { get; set; } = 1;

        public double Radius { get; set; }  
    }
}
