var _Map;
var _Markers;

export function Init(div, center, markers) {
    _Map = new ol.Map({
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM({
                    attributions: [ol.source.OSM.ATTRIBUTION, '<a href="https://www.teraword.net"><b>TeraWord</b></a>']
                })
            })
        ],
        target: div,
        view: new ol.View({
            center: ol.proj.fromLonLat(center),
           // maxZoom: 18,
            zoom: 12
        })
    });

    _Markers = new ol.layer.Vector({
        source: new ol.source.Vector(),
        style: new ol.style.Style({
            image: new ol.style.Icon({
                anchor: [256, 439],
                size: [800, 571],
                offset: [0, 0],
                opacity: 1,
                scale: 0.1,  
                anchorXUnits: 'pixels', // pixels fraction
                anchorYUnits: 'pixels', // pixels fraction
                src: './_content/teraword.blazor.openlayers/img/pin.png'
            }),
        }),
    });

    _Map.addLayer(_Markers);

    Markers(markers);
}

export function Center(center) {
    //_Map.getView().fit(bounds, map.getSize());
    _Map.getView().setCenter(ol.proj.fromLonLat(center));
    //_Map.getView().setZoom(5);
}

export function Markers(markers) {
    _Markers.getSource().clear();

    markers.forEach((point) => {
        var marker = new ol.Feature({
            geometry: new ol.geom.Point(ol.proj.fromLonLat(point)),
            name: 'Point'
        });

        _Markers.getSource().addFeature(marker);
    });
}