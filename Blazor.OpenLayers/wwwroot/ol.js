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
            zoom: zoom
        })
    });

    _Markers = new ol.layer.Vector({
        source: new ol.source.Vector()
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

function PinStyle(marker) {
    return [
        new ol.style.Style({
            image: new ol.style.Icon({
                anchor: [256, 439],
                size: [800, 571],
                offset: [0, 0],
                opacity: 1,
                scale: 0.1,
                color: marker.color,
                anchorXUnits: 'pixels', // pixels fraction
                anchorYUnits: 'pixels', // pixels fraction
                src: './_content/teraword.blazor.openlayers/img/pin.png'
            }),
        })
    ];
}

function FlagStyle(marker) {
    return [
        new ol.style.Style({
            fill: new ol.style.Fill({
                color: 'rgba(255,255,255,0.4)'
            }),
            stroke: new ol.style.Stroke({
                color: '#3399CC',
                width: 1.25
            }),
            text: new ol.style.Text({
                font: '18px Calibri,sans-serif',
                fill: new ol.style.Fill({ color: '#000' }),
                stroke: new ol.style.Stroke({
                    color: '#fff', width: 2
                }),
                // get the text from the feature - `this` is ol.Feature
                // and show only under certain resolution
                text: marker.title // map.getView().getZoom() > 12 ? marker.text : ''
            })
        })
    ];
}

// from http://viglino.github.io/OpenLayers-ext/examples/map.awesome.html

function AwesomeStyle(marker) {
    return [
        new ol.style.Style({
            text: new ol.style.Text({
                text: '\uf007',
                scale: 1.5,
                font: '900 18px "Font Awesome 5 Free"',
                textBaseline: 'bottom',
                fill: new ol.style.Fill({
                    color: marker.color
                }),
                stroke: new ol.style.Stroke({ color: 'black', width: 1 })
            })
        })
    ];
}

export function Markers(markers) {
    var source = _Markers.getSource();

    source.clear();

    markers.forEach((marker) => {
        var feature = new ol.Feature({
            geometry: new ol.geom.Point(ol.proj.fromLonLat(marker.coordinates)),
            name: marker.type
        });

        switch (marker.type) {
            case "Pin":
                feature.setStyle(PinStyle(marker));
                break;

            case "Flag":
                feature.setStyle(FlagStyle(marker));
                break;

            case "Awesome":
                feature.setStyle(AwesomeStyle(marker));
                break;
        }

        source.addFeature(feature);
    });
}