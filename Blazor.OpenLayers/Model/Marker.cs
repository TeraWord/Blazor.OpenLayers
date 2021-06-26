using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{ 
    public class Marker : Shape
    {
        public Marker() { Geometry = new GeometryPoint(); }

        public Marker(Point point) { Geometry = new GeometryPoint(); Point = point; }

        public Point Point { get => ((GeometryPoint)Geometry).Point; set => ((GeometryPoint)Geometry).Point = value; }

        public int Icon { get; set; }
    }
}
