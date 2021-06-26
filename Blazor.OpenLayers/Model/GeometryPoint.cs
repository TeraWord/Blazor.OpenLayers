using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class GeometryPoint : Geometry
    {
        public GeometryPoint() { Type = "Point"; }

        public override dynamic Coordinates { get => Point.Coordinates; }

        public Point Point { get; set; } 
    }
}
