using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{ 
    public class MarkerPin : Marker
    {
        public MarkerPin() : base() { }

        public MarkerPin(Point point) : base(point) { }

        public MarkerPin(Point point, string color) : base(point) { Color = color; } 
    }
}
