using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class ShapeLine : Shape
    {
        public ShapeLine() { Geometry = new Geometry("LineString"); }

        public ShapeLine(params Point[] point)
        {
            Geometry = new Geometry("LineString");
            Points = new List<Point>(point);
        }
              
        public IEnumerable<Point> Points { get => _points; set { _points = value; Geometry.Coordinates = value?.Select(x => x.Coordinates); } }
        private IEnumerable<Point> _points;

    }
}
