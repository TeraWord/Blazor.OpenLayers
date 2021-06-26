using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Geometry
    {
        public Geometry() { }

        public string Type { get; set; }

        public virtual dynamic Coordinates { get; set; }
    }
}
