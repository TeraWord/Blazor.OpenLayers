using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class GeometryCircle : Geometry
    {
        public GeometryCircle() { }

        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="center">Center</param>
        /// <param name="radius">Radius in km</param>
        public GeometryCircle(Point center, double radius)
        {
            Points = new List<Point>() { center };
            Radius = radius * 1000;
        }
    }
}
