using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeraWord.Blazor.OpenLayers;

namespace Demo
{
    public class LayerGoogle : Layer
    { 
        public LayerGoogle()
        {
            Kind = LayerKind.Tile;
            Url = "http://mt{0-3}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}";
            Attributions = "Google";
        }
    }
}
