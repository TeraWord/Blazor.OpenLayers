using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class ShapeCircle : Shape
    {
        public ShapeCircle() { Geometry = new GeometryPoint(); }

        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="center">Center</param>
        /// <param name="radius">Radius in km</param>
        public ShapeCircle(Point center, double radius)
        {
            Point = center;
            Radius = radius * 1000;
        }

        public Point Point { get => ((GeometryPoint)Geometry).Point; set => ((GeometryPoint)Geometry).Point = value; }
    }
}
