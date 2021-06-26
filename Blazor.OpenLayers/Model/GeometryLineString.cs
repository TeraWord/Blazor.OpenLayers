using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class GeometryLineString: Geometry
    {
        public GeometryLineString() { Type = "LineString"; }

        public override dynamic Coordinates { get => Points.Select(x => x.Coordinates); }

        public IEnumerable<Point> Points { get; set; } 
    }
}
