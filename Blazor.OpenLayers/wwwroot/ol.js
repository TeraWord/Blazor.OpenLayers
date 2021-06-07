var _Map;
var _Markers;

export function Init(div, center, zoom, markers, attributions) {
    _Map = new ol.Map({
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM({
                    attributions: [ol.source.OSM.ATTRIBUTION, attributions]
                })
            })
        ],
        target: div,
        view: new ol.View({
            center: ol.proj.fromLonLat(center.coordinates),
            // maxZoom: 18,
            zoom: zoom
        })
    });

    _Markers = new ol.layer.Vector({
        source: new ol.source.Vector(),
        //style: new ol.style.Style({
        //    image: new ol.style.Icon({
        //        anchor: [256, 439],
        //        size: [800, 571],
        //        offset: [0, 0],
        //        opacity: 1,
        //        scale: 0.1,
        //        color: '#BADA55',
        //        anchorXUnits: 'pixels', // pixels fraction
        //        anchorYUnits: 'pixels', // pixels fraction
        //        src: './_content/teraword.blazor.openlayers/img/pin.png'
        //    }),
        //}),
    });

    _Map.addLayer(_Markers);

    Markers(markers);
}

export function Center(point) {
    //_Map.getView().fit(bounds, map.getSize());
    _Map.getView().setCenter(ol.proj.fromLonLat(point.coordinates));
}

export function Zoom(zoom) {
    _Map.getView().setZoom(zoom);
}

export function Markers(markers) {
    var source = _Markers.getSource();

    source.clear();

    markers.forEach((point) => {
        var marker = new ol.Feature({
            geometry: new ol.geom.Point(ol.proj.fromLonLat(point.coordinates)),
            name: 'Point'
        });

        marker.setStyle(
            new ol.style.Style({
                image: new ol.style.Icon({
                    anchor: [256, 439],
                    size: [800, 571],
                    offset: [0, 0],
                    opacity: 1,
                    scale: 0.1,
                    color: point.color,
                    anchorXUnits: 'pixels', // pixels fraction
                    anchorYUnits: 'pixels', // pixels fraction
                    src: './_content/teraword.blazor.openlayers/img/pin.png'
                }),
            })
        );

        source.addFeature(marker);
    });
}