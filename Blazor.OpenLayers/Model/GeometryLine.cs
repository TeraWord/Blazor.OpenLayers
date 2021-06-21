using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class GeometryLine : Geometry
    {
        public GeometryLine() { }

        public GeometryLine(params Point[] point)
        {
            Points = new List<Point>(point);
        } 
    }
}
