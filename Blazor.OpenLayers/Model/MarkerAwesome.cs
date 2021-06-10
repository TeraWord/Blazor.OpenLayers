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

        public MarkerAwesome(Point point, int icon) : base(point) { Type = "Awesome"; Icon = icon; }

        public string Color { get; set; } = "#FFFFFF";

        public string BorderColor { get; set; } = "#FFFFFF";

        public string BackgroundColor { get; set; } = "#FF0000";

        public int Icon { get; set; }
    }
}
