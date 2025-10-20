using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class ShapeCircle : Shape
    {
        public ShapeCircle() { Geometry = new Geometry("Point"); }

        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="center">Center</param>
        /// <param name="radius">Radius in km</param>
        public ShapeCircle(Point center, double radius)
        {
            Geometry = new Geometry("Point");
            Point = center;
            Radius = radius * 1000;
        }

        public Point Point { get => _point; set { _point = value; Geometry.Coordinates = value?.Coordinates; } }
        private Point _point;
    }
}
