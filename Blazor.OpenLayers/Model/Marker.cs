using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{ 
    public class Marker : Feature
    {
        public Marker() { }

        public Marker(Point point) { Point = point; }

        public Point Point { get; set; } = new();
    }
}
