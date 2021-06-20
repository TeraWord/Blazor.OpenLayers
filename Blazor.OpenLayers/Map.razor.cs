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

        [Parameter] public ObservableCollection<Line> Lines { get; set; }

        [Parameter] public bool ShowPopup { get; set; }

        [Parameter] public string PopupID { get; set; }

        private string MapID { get; set; }

        private string InternalPopupID { get; set; }

        private IJSObjectReference Module { get; set; }

        public Map()
        {
            MapID = Guid.NewGuid().ToString();      
            InternalPopupID = Guid.NewGuid().ToString();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Center ??= new Point { Latitude = 39.2236, Longitude = 9.1181 };
            Markers ??= new ObservableCollection<object>();
            Lines ??= new ObservableCollection<Line>();

            Markers.CollectionChanged += Markers_CollectionChanged;
            Lines.CollectionChanged += Lines_CollectionChanged;
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/teraword.blazor.openlayers/MapOL.js");

                var popupID = string.IsNullOrWhiteSpace(PopupID) ? InternalPopupID : PopupID; 

                if (Module is not null) await Module.InvokeVoidAsync("MapOLInit", MapID, popupID, Center, Zoom, Markers, Lines, Attributions);
            }     
        }

        private void Markers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetMarkers(Markers);
        }

        private void Lines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetLines(Lines);
        }

        private async void SetMarkers(ObservableCollection<object> markers)
        {
            if (Module is not null) await Module.InvokeVoidAsync("MapOLMarkers", MapID, markers);
        }

        private async void SetLines(ObservableCollection<Line> lines)
        {
            if (Module is not null) await Module.InvokeVoidAsync("MapOLLines", MapID, lines);
        }

        private async void SetCenter(Point center)
        {
            if (Module is not null) await Module.InvokeVoidAsync("MapOLCenter", MapID, center);
        }

        private async void SetZoom(double zoom)
        {
            if (Module is not null) await Module.InvokeVoidAsync("MapOLZoom", MapID, zoom);
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
