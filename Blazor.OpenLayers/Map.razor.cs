﻿using Microsoft.AspNetCore.Components;
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

        [Parameter] public ObservableCollection<Geometry> Geometries { get; set; }

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
            Geometries ??= new ObservableCollection<Geometry>();

            Markers.CollectionChanged += Markers_CollectionChanged;
            Geometries.CollectionChanged += Geometries_CollectionChanged;
        }
        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/teraword.blazor.openlayers/MapOL.js");

                var popupID = string.IsNullOrWhiteSpace(PopupID) ? InternalPopupID : PopupID; 

                if (Module is not null) await Module.InvokeVoidAsync("MapOLInit", MapID, popupID, Center, Zoom, Markers, Geometries, Attributions);
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

        private async void SetMarkers(ObservableCollection<object> markers)
        {
            if (Module is not null) await Module.InvokeVoidAsync("MapOLMarkers", MapID, markers);
        }

        private async void SetGeometries(ObservableCollection<Geometry> geometries)
        {
            if (Module is not null) await Module.InvokeVoidAsync("MapOLGeometries", MapID, geometries);
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
