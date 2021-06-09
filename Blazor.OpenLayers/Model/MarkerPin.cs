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
        public MarkerPin() { Type = "Pin"; }

        public MarkerPin(Point point) : base(point) { Type = "Pin"; }

        public MarkerPin(Point point, string color) : base(point) { Type = "Pin"; Color = color; }

        public virtual string Color { get; set; }
    }
}
