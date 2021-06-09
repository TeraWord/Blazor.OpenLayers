using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeraWord.Blazor.OpenLayers;

namespace Demo.Pages
{
    public partial class Index
    {
        public Point Center { get; set; }

        private Map Map { get; set; }

        private static Random rnd = new();

        public Point NewPoint(Point around, double delta = 0.05)
        {
            return new Point
            {
                Latitude = around.Latitude + (rnd.NextDouble() - 0.5) * delta,
                Longitude = around.Longitude + (rnd.NextDouble() - 0.5) * delta,
            };
        }

        public Marker NewPin(Point around, string color, double delta = 0.05)
        {
            return new MarkerPin
            {
                Latitude = around.Latitude + (rnd.NextDouble() - 0.5) * delta,
                Longitude = around.Longitude + (rnd.NextDouble() - 0.5) * delta,
                Color = color
            };
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Center = new Point { Latitude = 39.2236, Longitude = 9.1181 };

                Map.Markers.Add(new MarkerPin(Center) { Color = "#00FF00" });
            }
        }

        private void OnCenterClick(dynamic e)
        {
            Center = new Point { Latitude = 39.2236, Longitude = 9.1181 }; // Cagliari
        }

        private void OnPinClick(dynamic e)
        {
            var color = (rnd.Next(100) % 2) switch { 0 => "#FF0000", 1 => "#00FF00", _ => "#0000FF" };
            Map.Markers.Add(NewPin(Center, color));
        }
        private void OnFlagClick(dynamic e)
        {
            Map.Markers.Add(new MarkerFlag(NewPoint(Center), "Ciao"));
        }

        private void OnAwesomeClick(dynamic e)
        {
            Map.Markers.Add(new MarkerAwesome(NewPoint(Center), "poop"));
        }
    }
}
