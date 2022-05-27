using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TeraWord.Blazor.OpenLayers;

namespace Demo.Pages
{
    public partial class GeoJson
    {
        private Map Map { get; set; }

        private string ClickString { get; set; }

        private string GeoJsonUrl { get; set; } =
            //"http://data-opendata.regione.sardegna.it/beniculturali/BBCC_MUS_0002/piano_scavi_siti.geojson";
            "http://data-opendata.regione.sardegna.it/beniculturali/BBCC_MUS_0007/albo_istituti_luoghi.geojson";

        private async Task OnLoadClick(dynamic e)
        {
            Map.Defaults.AutoPopup = true;
            Map.Defaults.PopupTitleProperty = "Denominazione_ufficiale";
            Map.Defaults.PopupContentProperty = "Indirizzo";
            Map.Defaults.Label = char.ConvertFromUtf32(0xF19C);
            Map.SetDefaults(Map.Defaults);

            using var client = new HttpClient();
            
            var json = await client.GetStringAsync(GeoJsonUrl);
            var obj = JsonSerializer.Deserialize<object>(json);

            Map.LoadGeoJson(obj);
        }

        private void OnFeatureClick(Feature feature)
        {
            ClickString = JsonSerializer.Serialize(feature, new JsonSerializerOptions { MaxDepth = 0, WriteIndented = true });
        }
    }
}
