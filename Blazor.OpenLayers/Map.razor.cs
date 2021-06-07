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

        [Parameter] public GeoPoint Center { get => _Center; set { _Center = value; GotoCenter(value); } }
        private GeoPoint _Center;

        [Parameter] public ObservableCollection<GeoPoint> Markers { get; set; }

        private string Div { get; set; }

        private IJSObjectReference Module;

        public Map()
        {
            Div = Guid.NewGuid().ToString();            
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Center ??= new GeoPoint { Latitude = 39.2236, Longitude = 9.1181 };
            Markers ??= new ObservableCollection<GeoPoint>();

            Markers.CollectionChanged += Markers_CollectionChanged;
        }

        private async void Markers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Module is not null) await Module.InvokeVoidAsync("Markers", Markers.Select(x => x.AsOpenLayers));
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                Module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/teraword.blazor.openlayers/ol.js");

                if (Module is not null) await Module.InvokeVoidAsync("Init", Div, Center.AsOpenLayers, Markers.Select(x => x.AsOpenLayers));
            }
            else
            {
                //if (Module is not null) await Module.InvokeVoidAsync("Center", Center.AsOpenLayers);
            }            
        }

        public async void GotoCenter(GeoPoint center)
        {
            if (Module is not null) await Module.InvokeVoidAsync("Center", center.AsOpenLayers);
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
