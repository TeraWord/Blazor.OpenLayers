using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Line : Feature
    {
        public Line() { }

        public Line(params Point[] point)
        {
            Points = new List<Point>(point);
        }

        public IEnumerable<Point> Points = new List<Point>();

        public int Width { get; set; } = 1;

        public IEnumerable<double[]> Coordinates { get => Points.Select(x => x.Coordinates); }

        public virtual string Label { get; set; } = "";
    }
}
