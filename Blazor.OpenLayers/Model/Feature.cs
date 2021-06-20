using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Feature
    {        
        public string Type => this.GetType().Name;

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";

        public string Color { get; set; } = "#FFFFFF";

        public double TextScale { get; set; } = 1;

        public string BackgroundColor { get; set; } = "#000000";
    }
}
