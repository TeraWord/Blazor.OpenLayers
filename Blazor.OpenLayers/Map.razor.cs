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

        [Parameter] public Point Center { get => _center; set => SetCenter(value); }
        private Point _center;

        [Parameter] public string Attributions { get; set; } = "<a href='https://www.teraword.net'><b>TeraWord</b></a>";

        [Parameter] public double Zoom { get => _zoom; set => SetZoom(value); } 
        private double _zoom = 14;

        [Parameter] public ObservableCollection<object> Markers { get; set; }

        [Parameter] public ObservableCollection<Geometry> Geometries { get; set; }

        [Parameter] public bool ShowPopup { get; set; }

        [Parameter] public string PopupID { get; set; }

        [Parameter] public EventCallback<Marker> OnMarkerClick { get; set; }

        [Parameter] public EventCallback<Geometry> OnGeometryClick { get; set; }

        [Parameter] public EventCallback<Point> OnClick { get; set; }

        private string MapID { get; set; }

        private string InternalPopupID { get; set; }

        private IJSObjectReference Module { get; set; }

        private DotNetObjectReference<Map> Instance { get; set; }

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
            Geometries ??= new ObservableCollection<Geometry>();

            Markers.CollectionChanged += Markers_CollectionChanged;
            Geometries.CollectionChanged += Geometries_CollectionChanged;
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                if (Module is null) Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/teraword.blazor.openlayers/MapOL.js");
                if (Instance is null) Instance = DotNetObjectReference.Create(this);
               
                var popupID = string.IsNullOrWhiteSpace(PopupID) ? InternalPopupID : PopupID; 

                if (Module is not null) await Module.InvokeVoidAsync("MapOLInit", MapID, popupID, Center, Zoom, Markers, Geometries, Attributions, Instance);
            }     

        }

        private void Markers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetMarkers(Markers);
        }

        private void Geometries_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetGeometries(Geometries);
        }

        [JSInvokable]
        public async Task OnInternalMarkerClick(Marker marker)
        {
            await OnMarkerClick.InvokeAsync(marker);
        }

        [JSInvokable]
        public async Task OnInternalGeometryClick(Geometry geometry)
        {
            await OnGeometryClick.InvokeAsync(geometry);
        }

        [JSInvokable]
        public async Task OnInternalClick(Point point)
        {
            await OnClick.InvokeAsync(point);
        }

        private async void SetMarkers(ObservableCollection<object> markers)
        {
            if (Module is not null) await Module.InvokeVoidAsync("MapOLMarkers", MapID, markers);
        }

        private async void SetGeometries(ObservableCollection<Geometry> geometries)
        {
            if (Module is not null) await Module.InvokeVoidAsync("MapOLGeometries", MapID, geometries);
        }

        public async void SetCenter(Point center)
        {
            _center = center;
            if (Module is not null) await Module.InvokeVoidAsync("MapOLCenter", MapID, center);
        }

        public async void SetZoom(double zoom)
        {
            _zoom = zoom;
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
