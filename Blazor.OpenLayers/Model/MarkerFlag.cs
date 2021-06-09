using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{ 
    public class MarkerFlag : Marker
    {
        public MarkerFlag() { Type = "Flag"; }

        public MarkerFlag(Point point) : base(point) { Type = "Flag"; }

        public MarkerFlag(Point point, string title) : base(point) { Type = "Flag"; Title = title; }

        public virtual string Title { get; set; }
    }
}
