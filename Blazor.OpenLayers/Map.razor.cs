using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public partial class Map : IAsyncDisposable
    {
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [Parameter] public Point Center { get => _center; set { _center = value; SetCenter(value); } }
        private Point _center;

        [Parameter] public string Attributions { get; set; } = "<a href='https://www.teraword.net'><b>TeraWord</b></a>";

        [Parameter] public double Zoom { get => _zoom; set { _zoom = value; SetZoom(value); } } 
        private double _zoom = 14;

        [Parameter] public ObservableCollection<object> Markers { get; set; }

        private string Div { get; set; }

        private IJSObjectReference Module;

        public Map()
        {
            Div = Guid.NewGuid().ToString();            
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Center ??= new Point { Latitude = 39.2236, Longitude = 9.1181 };
            Markers ??= new ObservableCollection<object>();

            Markers.CollectionChanged += Markers_CollectionChanged;
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/teraword.blazor.openlayers/ol.js");

                if (Module is not null) await Module.InvokeVoidAsync("Init", Div, Center, Zoom, Markers, Attributions);
            }
            else
            {
                //if (Module is not null) await Module.InvokeVoidAsync("Center", Center.AsOpenLayers);
            }            
        }

        private void Markers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetMarkers(Markers);
        }

        private async void SetMarkers(ObservableCollection<object> markers)
        {

#if DEBUG
            System.IO.File.WriteAllText("D:/Tests/Blazor.OpenLayers/Markers.json", System.Text.Json.JsonSerializer.Serialize(this, new() { WriteIndented = true }));
#endif

            if (Module is not null) await Module.InvokeVoidAsync("Markers", markers);
        }

        private async void SetCenter(Point center)
        {
            if (Module is not null) await Module.InvokeVoidAsync("Center", center);
        }

        private async void SetZoom(double zoom)
        {
            if (Module is not null) await Module.InvokeVoidAsync("Zoom", zoom);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            await Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (Markers is not null) Markers.CollectionChanged -= Markers_CollectionChanged;
            if (Module is not null) await Module.DisposeAsync();//.ConfigureAwait(false);

            Module = null;
        }
    }
}
