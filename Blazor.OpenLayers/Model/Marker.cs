﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{ 
    public class Marker : Point
    {
        public Marker() { }

        public Marker(Point point) : base(point) { }

        public string Type { get; internal set; }

        public string Title { get; set; } = "";

        public string Content { get; set; } = "";
    }
}
