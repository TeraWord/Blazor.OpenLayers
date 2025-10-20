using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Feature
    {
        public Feature() { } // { Type = this.GetType().Name; }

        public string Type { get; set; } = "Feature";

        public Geometry Geometry { get; set; }

        public Dictionary<string, dynamic> Properties { get; set; } = new();
    }
}
