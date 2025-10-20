using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public enum LayerKind { OSM, Tile }

    public class Layer
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public virtual LayerKind Kind { get; protected set; } 

        public string Url { get; protected set; }

        public string Attributions { get; protected set; }
    }
}
