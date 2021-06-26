using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class ShapeLine : Shape
    {
        public ShapeLine() { Geometry = new GeometryLineString(); }

        public ShapeLine(params Point[] point)
        {
            Points = new List<Point>(point);
        }

        public IEnumerable<Point> Points { get => ((GeometryLineString)Geometry).Points; set => ((GeometryLineString)Geometry).Points = value; }
    }
}
