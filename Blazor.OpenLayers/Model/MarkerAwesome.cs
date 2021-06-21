using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{ 
    public class MarkerAwesome : Marker
    {
        public MarkerAwesome() { }

        public MarkerAwesome(Point point) : base(point) { }

        public MarkerAwesome(Point point, int icon) : base(point) { Icon = icon; }
    }
}
