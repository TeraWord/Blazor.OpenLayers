using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TeraWord.Blazor.OpenLayers;

namespace Demo.Pages
{
    public partial class GeoJson
    {        
        private Map Map { get; set; }

        private string GeoJsonUrl { get; set; } = "json/albo_istituti_luoghi.json";

        private void OnLoadClick(dynamic e)
        {
            Map.LoadGeoJson(GeoJsonUrl);
        }
    }
}
