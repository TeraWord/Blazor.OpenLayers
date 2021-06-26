using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Shape : Feature
    {
        public Shape() { Kind = this.GetType().Name; ID = Guid.NewGuid(); }

        public string Kind { get; set; }

        public Guid ID { get; set; }

        public bool Popup { get; set; }

        public string Label { get; set; } = "";

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public double TextScale { get; set; } = 1;

        public string Color { get; set; } = "#FFFFFF";

        public string BorderColor { get; set; } = "#FFFFFF";

        public int BorderSize { get; set; } = 1;

        public string BackgroundColor { get; set; } = "#000000";

        public double Radius { get; set; }
    }
}
