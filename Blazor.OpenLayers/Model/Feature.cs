using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Feature
    {
        public Feature() {  }

        public string Type { get; set; }

        public Geometry Geometry { get; set; }

        public Dictionary<string, dynamic> Properties { get; set; }
    }
}
