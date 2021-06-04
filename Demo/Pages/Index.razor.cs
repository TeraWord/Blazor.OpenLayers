using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeraWord.Blazor.OpenLayers;

namespace Demo.Pages
{
    public partial class Index
    {
        public GeoPoint Center { get; set; }

        private Map Map { get; set; }

        private static Random rnd = new();

        private void OnCenterClick(dynamic e)
        {
            Center = new GeoPoint { Latitude = 39.2236, Longitude = 9.1181 };
        }

        private void OnMarkerClick(dynamic e)
        {
            Map.Markers.Add(new GeoPoint(39.2236 + (rnd.NextDouble()-0.5)*0.05, 9.1181 + (rnd.NextDouble()-0.5)*0.05));
        }
    }
}
