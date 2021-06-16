var _Map;
var _Markers;

export function Init(mapID, popupID, center, zoom, markers, attributions) {
    _Map = new ol.Map({
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM({
                    attributions: [ol.source.OSM.ATTRIBUTION, attributions]
                })
            })
        ],
        target: mapID,
        view: new ol.View({
            center: ol.proj.fromLonLat(center.coordinates),
            zoom: zoom
        })
    });

    _Markers = new ol.layer.Vector({
        source: new ol.source.Vector()
    });

    _Map.addLayer(_Markers);

    var popupElement = document.getElementById(popupID);

    var popup = new ol.Overlay({
        element: popupElement,
        positioning: 'bottom-center',
        stopEvent: false,
        offset: [0, -50],
    });

    _Map.addOverlay(popup);

    _Map.on('click', function (evt) { OnMapClick(evt, popup, popupElement) });
    _Map.on('pointermove', function (evt) { OnMapPointerMove(evt, popupElement) });

    Markers(markers);
}

export function Center(point) {
    //_Map.getView().fit(bounds, map.getSize());
    _Map.getView().setCenter(ol.proj.fromLonLat(point.coordinates));
}

export function Zoom(zoom) {
    _Map.getView().setZoom(zoom);
}

function OnMapClick(evt, popup, element) {
    var feature = _Map.forEachFeatureAtPixel(evt.pixel, function (feature) { return feature; });

    if (feature) {
        var coordinates = feature.getGeometry().getCoordinates();

        popup.setPosition(coordinates);

        $(element).popover({
            placement: 'top',
            html: true,
            title: feature.get("title"),
            content: feature.get('content'),
        });

        $(element).popover('show');
    } else {
        $(element).popover('dispose');
    }
}

function OnMapPointerMove(evt, element) {
    if (evt.dragging) {
        $(element).popover('dispose');
        return;
    }

    //var pixel = _Map.getEventPixel(evt.originalEvent);
    //var hit = _Map.hasFeatureAtPixel(pixel);

    //_Map.getTarget().style.cursor = hit ? 'pointer' : '';
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
    var width = 30;
    var height = 30;
    var font = 'bold 18px Arial';

    var canvas = document.createElement('canvas');
    var ctx = canvas.getContext("2d");

    ctx.font = font;
    width = ctx.measureText(marker.title).width;

    var context = ol.render.toContext(canvas.getContext('2d'), {
        size: [width, height],
        pixelRatio: 1,
        font: font
    });

    var strokeWidth = 2;
    var arrowWidth = 6;

    var symbol = [
        [0 + strokeWidth, 0 + strokeWidth],
        [width - strokeWidth, 0 + strokeWidth],
        [width - strokeWidth, height - strokeWidth - arrowWidth],
        [0 + strokeWidth + arrowWidth * 3, height - strokeWidth - arrowWidth],
        [0 + strokeWidth + arrowWidth * 2, height - strokeWidth],
        [0 + strokeWidth + arrowWidth * 1, height - strokeWidth - arrowWidth],
        [0 + strokeWidth, height - strokeWidth - arrowWidth],
        [0 + strokeWidth, 0 + strokeWidth]
    ];

    context.setStyle(
        new ol.style.Style({
            fill: new ol.style.Fill({
                color: "#ffcc66"
            }),
            stroke: new ol.style.Stroke({
                color: "#b37700",
                width: strokeWidth
            }),
        })
    );

    context.drawGeometry(new ol.geom.Polygon([symbol]));

    return [
        new ol.style.Style({
            image: new ol.style.Icon({
                anchorXUnits: 'pixels', // pixels fraction
                anchorYUnits: 'pixels', // pixels fraction
                anchor: symbol[4],
                size: [width, height],
                offset: [0, 0],
                img: canvas,
                imgSize: [width, height],   
                scale: 1,
            }),
        }),
        new ol.style.Style({
            text: new ol.style.Text({
                text: marker.title,                 
                offsetY: -height / 2,
                offsetX: -arrowWidth,
                textAlign: "left",
                opacity: 1,
                scale: 0.75,
                font: font,
                fill: new ol.style.Fill({
                    color: "#444444"
                })
            })
        })
    ];
}

function AwesomeStyle(marker) {
    return [
        new ol.style.Style({
            image: new ol.style.Icon({
                anchor: [0, 14],
                size: [56, 21],
                offset: [0, 0],
                opacity: 1,
                scale: 0.5,
                color: marker.color,
                anchorXUnits: 'pixels', // pixels fraction
                anchorYUnits: 'pixels', // pixels fraction
                src: './_content/teraword.blazor.openlayers/img/pin-back.png'
            }),
        }),
        new ol.style.Style({
            text: new ol.style.Text({
                text: String.fromCodePoint(0xF041), // Map Marker
                scale: 2,
                font: '900 18px "Font Awesome 5 Free"',
                textBaseline: 'bottom',
                fill: new ol.style.Fill({
                    color: marker.backgroundColor
                }),
                stroke: new ol.style.Stroke({
                    color: marker.borderColor,
                    width: 3
                })
            })
        }),
        new ol.style.Style({
            text: new ol.style.Text({
                text: String.fromCodePoint(marker.icon),
                offsetY: -22,
                opacity: 1,
                scale: 1,
                font: '900 18px "Font Awesome 5 Free"',
                fill: new ol.style.Fill({
                    color: marker.color
                })
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
            title: marker.title,
            content: marker.content
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