using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{ 
    public class MarkerFlag : Marker
    {
        public MarkerFlag() : base() { }

        public MarkerFlag(Point point) : base(point) { }

        public MarkerFlag(Point point, string title) : base(point) { Title = title; }
    }
}
