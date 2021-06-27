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

        private static Random rnd { get; set; } = new();

        private string ClickString { get; set; }

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
            //point = new Point(39.215704, 9.109290); // Cagliari incroco via Roma - via Sassari
            //Map.Markers.Add(new MarkerPin(point, "#FF0000"));

            var flag = new MarkerFlag(point, NewText);
            flag.BackgroundColor = "#00AA00EE";
            flag.BorderColor = "#000000FF";
            flag.TextScale = 1.2;
            flag.Popup = true;
            Map.Markers.Add(flag);
        }

        private string NewColor
        {
            get
            {
                var result = new string[] {
                    "#990000", "#009900", "#000099",
                    "#997700", "#009977", "#770099",
                    "#990077", "#779900", "#007799",
                };

                return result[rnd.Next(result.Length)];
            }
        }

        private string NewText
        {
            get
            {
                var result = new string[] {
                    "Turn a blind eye",
                    "White elephant",
                    "Crocodile tears",
                    "Diehard",
                    "Resting on laurels",
                    "Read the riot act",
                    "Paint the town red",
                    "Running amok",
                    "By and large",
                    "The third degree",
                };

                return result[rnd.Next(result.Length)];
            }
        }

        private int NewIcon
        {
            get
            {
                var result = new int[] {
                    0xF29A, 
                    0xF5A7, 
                    0xF0F0, 
                    0xF48E,
                };

                return result[rnd.Next(result.Length)];
            }
        }

        private void OnAwesomeClick(dynamic e)
        {
            var point = NewPoint();
            var color = NewColor;

            var marker = new MarkerAwesome(point)
            {
                Title = NewText,
                Content = $"<b>Colore:</b> {color}",
                Popup = true,
                Label = char.ConvertFromUtf32(NewIcon),
                BackgroundColor = color,
            };

            Map.Markers.Add(marker);
        }

        private void OnLineClick(dynamic e)
        {
            var a = NewPoint();
            var b = NewPoint();

            var line = new ShapeLine(a, b);
            var color = NewColor;

            line.Label = $"{a.DistanceTo(b)} km";
            line.TextScale = 1.5;
            line.BorderColor = color;
            line.BorderSize = 4;

            Map.Shapes.Add(line);

            var ma = new MarkerAwesome(a)
            {
                Title = "A",
                Content = $"<b>Punto:</b> A",
                Label = "A",
                BackgroundColor = color,
            };

            var mb = new MarkerAwesome(b)
            {
                Title = "B",
                Content = $"<b>Punto:</b> B",
                Label = "B",
                BackgroundColor = color,
            };

            Map.Markers.Add(ma);
            Map.Markers.Add(mb);
        }

        private void OnCircleClick(dynamic e)
        {
            var a = NewPoint();
            var b = NewPoint();

            var line = new ShapeLine(a, b);
            var color = NewColor;
            var distance = a.DistanceTo(b);

            line.Label = $"{distance} km";
            line.TextScale = 1.5;
            line.BorderColor = color;
            line.BorderSize = 2;

            Map.Shapes.Add(line);

            var circle = new ShapeCircle(a, distance);
            circle.Label = $"{distance} km";
            circle.TextScale = 1.5;
            circle.BackgroundColor = "#0000BB11";
            circle.BorderColor = "#0000BB66";

            Map.Shapes.Add(circle);
        }

        private void OnMarkerClick(Marker marker)
        {
            ClickString = JsonSerializer.Serialize(marker, new JsonSerializerOptions { MaxDepth = 0, WriteIndented = true });
        }

        private void OnShapeClick(Shape shape)
        {
            ClickString = JsonSerializer.Serialize(shape, new JsonSerializerOptions { MaxDepth = 0, WriteIndented = true });
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

        
    }
}
