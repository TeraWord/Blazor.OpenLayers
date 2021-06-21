using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TeraWord.Blazor.OpenLayers;

namespace Demo.Pages
{
    public partial class Index
    {
        public Point Center { get; set; }

        private Map Map { get; set; }

        private static Random rnd = new();

        public Point NewPoint(double delta = 0.05)
        {
            var around = Map.Center;

            return new Point
            {
                Latitude = around.Latitude + (rnd.NextDouble() - 0.5) * delta,
                Longitude = around.Longitude + (rnd.NextDouble() - 0.5) * delta,
            };
        }

        public Marker NewPin(string color, double delta = 0.05)
        {
            var around = Map.Center;

            var point = new Point
            {
                Latitude = around.Latitude + (rnd.NextDouble() - 0.5) * delta,
                Longitude = around.Longitude + (rnd.NextDouble() - 0.5) * delta
            };

            return new MarkerPin(point)
            {
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
            Map.Markers.Add(NewPin(color));
        }

        private void OnFlagClick(dynamic e)
        {
            var point = NewPoint();

            var testi = new string[] {
                "Ciao",
                "TeraWord",
                "Mario Rossi",

                "Testo riempitivo nelle prove grafiche",
                "|!£$%&/()=?^"
            };

            //point = new Point(39.215704, 9.109290); // Cagliari incroco via Roma - via Sassari

            Map.Markers.Add(new MarkerFlag(point, testi[rnd.Next(testi.Length)]));
        }

        private string NewColor
        {
            get
            {
                var colors = new string[] {
                    "#990000", "#009900", "#000099",
                    "#997700", "#009977", "#770099",
                    "#990077", "#779900", "#007799",
                };

                return colors[rnd.Next(colors.Length)];
            }
        }

        private void OnAwesomeClick(dynamic e)
        {
            var icons = new int[] { 0xF29A, 0xF5A7, 0xF0F0, 0xF48E };

            var titles = new string[] {
                "Uno", "Due", "Tre",
                "Quattro", "Cinque", "Sei",
                "Sette Otto", "Nove Dieci"
            };

            var point = NewPoint();
            var color = NewColor;

            var marker = new MarkerAwesome(point)
            {
                Title = titles[rnd.Next(titles.Length)],
                Content = $"<b>Colore:</b> {color}",
                Icon = icons[rnd.Next(icons.Length)],
                BackgroundColor = color,
            };

            Map.Markers.Add(marker);
        }

        private void OnLineClick(dynamic e)
        {
            var a = NewPoint();
            var b = NewPoint();

            var line = new GeometryLine(a, b);
            var color = NewColor;

            line.Label = $"{a.DistanceTo(b)} km";
            line.Width = 4;
            line.TextScale = 1.5;
            line.BackgroundColor = color;

            Map.Geometries.Add(line);

            var ma = new MarkerAwesome(a)
            {
                Title = "A",
                Content = $"<b>Punto:</b> A",
                Icon = 0x41,
                BackgroundColor = color,
            };

            var mb = new MarkerAwesome(b)
            {
                Title = "B",
                Content = $"<b>Punto:</b> B",
                Icon = 0x42,
                BackgroundColor = color,
            };

            Map.Markers.Add(ma);
            Map.Markers.Add(mb);
        }

        private void OnCircleClick(dynamic e)
        {
            var a = NewPoint();
            var b = NewPoint();

            var line = new GeometryLine(a, b);
            var color = NewColor;
            var distance = a.DistanceTo(b);

            line.Label = $"{distance} km";
            line.Width = 2;
            line.TextScale = 1.5;
            line.BackgroundColor = color;

            Map.Geometries.Add(line);

            var circle = new GeometryCircle(a, distance);
            circle.Color = "#0000BB11";
            circle.BorderColor = "#0000BB66";
            Map.Geometries.Add(circle);
        }

        private void OnMarkerClick(Marker marker)
        {
            ClickString = JsonSerializer.Serialize(marker, new JsonSerializerOptions { MaxDepth = 0, WriteIndented = true });
        }

        private void OnGeometryClick(Geometry geometry)
        {
            ClickString = JsonSerializer.Serialize(geometry, new JsonSerializerOptions { MaxDepth = 0, WriteIndented = true });
        }

        private void OnClick(Point point)
        {
            ClickString = JsonSerializer.Serialize(point, new JsonSerializerOptions { MaxDepth = 0, WriteIndented = true });
            Map.SetCenter(point);
        }

        private void OnZoomGeometriesClick(dynamic e)
        {
            Map.SetZoomToExtent(Extent.Geometries);
        }

        private void OnZoomMarkersClick(dynamic e)
        {
            Map.SetZoomToExtent(Extent.Markers);
        }

        private string ClickString { get; set; }
    }
}
