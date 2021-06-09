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
        public MarkerAwesome() { Type = "Awesome"; }

        public MarkerAwesome(Point point) : base(point) { Type = "Awesome"; }

        public MarkerAwesome(Point point, string icon) : base(point) { Type = "Awesome"; Icon = icon; }

        public string Color { get; set; } = "white";

        public string BorderColor { get; set; } = "white";

        public string backgroundColor { get; set; } = "red";

        public string Icon { get; set; }
    }
}
