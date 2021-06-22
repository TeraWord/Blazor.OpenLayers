using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Feature
    {
        public Feature() { Type = this.GetType().Name; ID = Guid.NewGuid(); }

        public Guid ID { get; set; }

        public string Type { get; set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";
        
        public double TextScale { get; set; } = 1;

        public string Color { get; set; } = "#FFFFFF";

        public string BorderColor { get; set; } = "#FFFFFF";

        public int BorderSize { get; set; } = 1;

        public string BackgroundColor { get; set; } = "#000000";
    }
}
