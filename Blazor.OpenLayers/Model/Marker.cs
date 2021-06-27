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
        public Marker() { Geometry = new Geometry("Point"); }

        public Marker(Point point) { Geometry = new Geometry("Point"); Point = point; }

        public Point Point { get => _point; set { _point = value; Geometry.Coordinates = value?.Coordinates; } }
        private Point _point;
    }
}
