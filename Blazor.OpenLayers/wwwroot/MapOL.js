var _MapOL = new Array();

export function MapOLInit(mapID, popupID, center, zoom, markers, attributions) {
    _MapOL[mapID] = new MapOL(mapID, popupID, center, zoom, markers, attributions);
}

export function MapOLCenter(mapID, point) {
    _MapOL[mapID].setCenter(point);
}

export function MapOLZoom(mapID, zoom) {
    _MapOL[mapID].setZoom(zoom);
}

export function MapOLMarkers(mapID, markers) {
    _MapOL[mapID].setMarkers(markers);
}

export function MapOLLines(mapID, lines) {
    _MapOL[mapID].setLines(lines);
}

// --- MapOL ----------------------------------------------------------------------------//

function MapOL(mapID, popupID, center, zoom, markers, lines, attributions) {
    this.Map = new ol.Map({
        layers: [
            new ol.layer.Tile({
                source: new ol.source.OSM({
                    attributions: [ol.source.OSM.ATTRIBUTION, attributions]
                })
            })
        ],
        target: mapID,
        view: new ol.View({
            center: ol.proj.transform(center.coordinates, 'EPSG:4326', 'EPSG:3857'),
            zoom: zoom
        })
    });

    this.Lines = new ol.layer.Vector({
        source: new ol.source.Vector()
    });

    this.Map.addLayer(this.Lines);

    this.Markers = new ol.layer.Vector({
        source: new ol.source.Vector()
    });

    this.Map.addLayer(this.Markers);    

    var popupElement = document.getElementById(popupID);

    var popup = new ol.Overlay({
        element: popupElement,
        positioning: 'bottom-center',
        stopEvent: false,
        offset: [0, -50],
    });

    this.Map.addOverlay(popup);

    var that = this;

    this.Map.on('click', function (evt) { that.onMapClick(evt, popup, popupElement) });
    this.Map.on('pointermove', function (evt) { that.onMapPointerMove(evt, popupElement) });

    this.setMarkers(markers);
    this.setLines(lines);
}

MapOL.prototype.setMarkers = function (markers) {
    var source = this.Markers.getSource();

    source.clear();

    markers.forEach((marker) => {
        var feature = new ol.Feature({
            geometry: new ol.geom.Point(ol.proj.transform(marker.point.coordinates, 'EPSG:4326', 'EPSG:3857')),
            title: marker.title,
            content: marker.content
        });

        switch (marker.type) {
            case "MarkerPin":
                feature.setStyle(this.pinStyle(marker));
                break;

            case "MarkerFlag":
                feature.setStyle(this.flagStyle(marker));
                break;

            case "MarkerAwesome":
                feature.setStyle(this.awesomeStyle(marker));
                break;
        }

        source.addFeature(feature);
    });
}

MapOL.prototype.setLines = function (lines) {
    var source = this.Lines.getSource();

    source.clear();

    if (!lines) return;

    lines.forEach((line) => {
        for (var i = 0; i < line.coordinates.length; i++) {
            line.coordinates[i] = ol.proj.transform(line.coordinates[i], 'EPSG:4326', 'EPSG:3857');
        }

        var feature = new ol.Feature({
            geometry: new ol.geom.LineString(line.coordinates),
            title: line.title,
            content: line.content
        });

        switch (line.type) {
            case "Line":
                feature.setStyle(this.lineStyle(line));
                break;
        }

        source.addFeature(feature);
    });
}

MapOL.prototype.setZoom = function (zoom) {
    this.Map.getView().setZoom(zoom);
}

MapOL.prototype.setCenter = function (point) {
    this.Map.getView().setCenter(ol.proj.transform(point.coordinates, 'EPSG:4326', 'EPSG:3857'));
}

MapOL.prototype.onMapClick = function (evt, popup, element) {
    $(element).popover('dispose');

    var feature = this.Map.forEachFeatureAtPixel(evt.pixel, function (feature) { return feature; });

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

MapOL.prototype.onMapPointerMove = function (evt, element) {
    if (evt.dragging) {
        $(element).popover('dispose');
        return;
    }

    //var pixel = _Map.getEventPixel(evt.originalEvent);
    //var hit = _Map.hasFeatureAtPixel(pixel);

    //_Map.getTarget().style.cursor = hit ? 'pointer' : '';
}

MapOL.prototype.pinStyle = function (marker) {
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

MapOL.prototype.flagStyle = function (marker) {
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

MapOL.prototype.awesomeStyle = function (marker) {
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

MapOL.prototype.lineStyle = function (line) {
    return [
        new ol.style.Style({
            //fill: new ol.style.Fill({ color: line.color, width: line.width }),
            stroke: new ol.style.Stroke({ color: line.backgroundColor, width: line.width })
        }),
        new ol.style.Style({
            text: new ol.style.Text({
                text: line.label,
                placement: "line",
                opacity: 1,
                scale: line.textScale,
                fill: new ol.style.Fill({
                    color: line.color
                }),
                stroke: new ol.style.Stroke({ color: line.backgroundColor, width: line.width })
            }),
           
        })
    ];
}
