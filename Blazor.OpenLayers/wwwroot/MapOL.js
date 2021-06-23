var _MapOL = new Array();

export function MapOLInit(mapID, popupID, center, zoom, markers, geometries, attributions, instance) {
    _MapOL[mapID] = new MapOL(mapID, popupID, center, zoom, markers, geometries, attributions, instance);
}

export function MapOLCenter(mapID, point) {
    _MapOL[mapID].setCenter(point);
}

export function MapOLZoom(mapID, zoom) {
    _MapOL[mapID].setZoom(zoom);
}

export function MapOLZoomToExtent(mapID, extent) {
    _MapOL[mapID].setZoomToExtent(extent);
}

export function MapOLMarkers(mapID, markers) {
    _MapOL[mapID].setMarkers(markers);
}

export function MapOLGeometries(mapID, geometries) {
    _MapOL[mapID].setGeometries(geometries);
}

// --- MapOL ----------------------------------------------------------------------------//

function MapOL(mapID, popupID, center, zoom, markers, geometries, attributions, instance) {
    this.Instance = instance;

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

    this.Geometries = new ol.layer.Vector({
        source: new ol.source.Vector()
    });

    this.Map.addLayer(this.Geometries);

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
    this.setGeometries(geometries);
}

MapOL.prototype.setMarkers = function (markers) {
    var source = this.Markers.getSource();

    source.clear();

    markers.forEach((marker) => {
        var feature = new ol.Feature({
            geometry: new ol.geom.Point(ol.proj.transform(marker.point.coordinates, 'EPSG:4326', 'EPSG:3857')),
            popup: marker.popup,
            title: marker.title,
            content: marker.content
        });

        feature.marker = marker;

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

MapOL.prototype.setGeometries = function (geometries) {
    var source = this.Geometries.getSource();

    source.clear();

    if (!geometries) return;

    geometries.forEach((geometry) => {
        for (var i = 0; i < geometry.coordinates.length; i++) {
            geometry.coordinates[i] = ol.proj.transform(geometry.coordinates[i], 'EPSG:4326', 'EPSG:3857');
        }
                
        var feature;

        switch (geometry.type) {
            case "GeometryLine":
                feature = new ol.Feature({
                    geometry: new ol.geom.LineString(geometry.coordinates),
                    popup: geometry.popup,
                    title: geometry.title,
                    content: geometry.content
                });
                break;

            case "GeometryCircle":
                var circle = new ol.geom.Circle(geometry.coordinates[0], geometry.radius / ol.proj.getPointResolution('EPSG:3857', 1, geometry.coordinates[0]));

                feature = new ol.Feature({
                    //radius / ol.proj.getPointResolution('EPSG:3857', 1, feature.getGeometry().getCoordinates()
                    //geometry: new ol.geom.Circle(geometry.coordinates[0], geometry.radius / ol.proj.getPointResolution('EPSG:3857', 1, geometry.coordinates[0])),
                    geometry: new ol.geom.Polygon.fromCircle(circle, 32, 90),
                    popup: geometry.popup,
                    title: geometry.title,
                    content: geometry.content
                });
                break;
        }

        feature.geometry = geometry;

        switch (geometry.type) {
            case "GeometryLine":
                feature.setStyle(this.lineStyle(geometry));
                break;

            case "GeometryCircle":
                feature.setStyle(this.circleStyle(geometry));
                break;
        }

        source.addFeature(feature);
    });
}

MapOL.prototype.setZoom = function (zoom) {
    this.Map.getView().setZoom(zoom);
}

MapOL.prototype.setZoomToExtent = function (extent) {
    switch (extent) {
        case "Markers":
            var extent = this.Markers.getSource().getExtent();
            if (extent[0] === Infinity) return;
            this.Map.getView().fit(extent, this.Map.getSize());
            break;

        case "Geometries":
            var extent = this.Geometries.getSource().getExtent();
            if (extent[0] === Infinity) return;
            this.Map.getView().fit(extent, this.Map.getSize());
            break;
    }
}

MapOL.prototype.setCenter = function (point) {
    this.Map.getView().setCenter(ol.proj.transform(point.coordinates, 'EPSG:4326', 'EPSG:3857'));
}

MapOL.prototype.onMapClick = function (evt, popup, element) {
    $(element).popover('dispose');

    var that = this;
    var showedPopup = false;

    this.Map.forEachFeatureAtPixel(evt.pixel, function (feature) {
        if (feature.marker != null) that.Instance.invokeMethodAsync('OnInternalMarkerClick', feature.marker);
        if (feature.geometry != null) that.Instance.invokeMethodAsync('OnInternalGeometryClick', feature.geometry);

        var showPopup = feature.get("popup");
        var title = feature.get("title");
        var content = feature.get('content');
       
        if (showPopup && title != "") {
            var coordinates = feature.getGeometry().getCoordinates();

            popup.setPosition(coordinates);

            $(element).popover({
                placement: 'top',
                html: true,
                title: title,
                content: content,
            });

            $(element).popover('show');
            showedPopup = true;
        }
    });

    if (!showedPopup) {
        var coordinate = ol.proj.transform(evt.coordinate, 'EPSG:3857', 'EPSG:4326')
        var point = { Latitude: coordinate[1], Longitude: coordinate[0] };

        this.Instance.invokeMethodAsync('OnInternalClick', point);
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
    return new ol.style.Style({
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
    });
}

MapOL.prototype.flagStyle = function (marker) {
    var padTop = 4;
    var padBottom = 2;
    var padLeft = 5;
    var padRight = 5;

    var size = 10;
    var width = size;
    var height = size;

    var canvas = document.createElement('canvas');
    var ctx = canvas.getContext("2d");

    var context = ol.render.toContext(canvas.getContext('2d'), {
        size: [width, height],
        pixelRatio: 1
    });
       
    var symbol = [
        [0, 0],
        [width, 0],
        [width /2, height],
        [0, 0]
    ];

    context.setStyle(
        new ol.style.Style({
            fill: new ol.style.Fill({ color: marker.backgroundColor }),
            stroke: new ol.style.Stroke({ color: marker.borderColor, width: marker.borderSize }),
        })
    );

    context.drawGeometry(new ol.geom.Polygon([symbol]));

    return new ol.style.Style({
        image: new ol.style.Icon({
            anchorXUnits: 'pixels', // pixels fraction
            anchorYUnits: 'pixels', // pixels fraction
            anchor: [width / 2, height],
            size: [width, height],
            offset: [0, 0],
            img: canvas,
            imgSize: [width, height],
            scale: 1,
        }),
        text: new ol.style.Text({
            text: marker.title,
            offsetY: -size - padBottom + 1,
            offsetX: -size,
            textAlign: "left",
            textBaseline: "bottom",
            scale: marker.textScale,
            //font: "bold 18px Arial",
            fill: new ol.style.Fill({ color: marker.color }),
            //stroke: new ol.style.Stroke({ color: marker.borderColor, width: marker.borderSize }),
            backgroundFill: new ol.style.Fill({ color: marker.backgroundColor }),
            backgroundStroke: new ol.style.Stroke({ color: marker.borderColor, width: marker.borderSize }),
            padding: [padTop, padRight, padBottom, padLeft]
        })
    });
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
                fill: new ol.style.Fill({ color: marker.backgroundColor }),
                stroke: new ol.style.Stroke({ color: marker.borderColor, width: 3 })
            })
        }),
        new ol.style.Style({
            text: new ol.style.Text({
                text: String.fromCodePoint(marker.icon),
                offsetY: -22,
                opacity: 1,
                scale: 1,
                font: '900 18px "Font Awesome 5 Free"',
                fill: new ol.style.Fill({ color: marker.color })
            })
        })
    ];
}

MapOL.prototype.lineStyle = function (line) {
    return new ol.style.Style({
        stroke: new ol.style.Stroke({ color: line.borderColor, width: line.borderSize }),
        text: new ol.style.Text({
            text: line.label,
            placement: "line",
            opacity: 1,
            scale: line.textScale,
            fill: new ol.style.Fill({ color: line.color }),
            stroke: new ol.style.Stroke({ color: line.borderColor, width: line.borderSize })
        }),
    });
}

MapOL.prototype.circleStyle = function (circle) {
    return new ol.style.Style({
        fill: new ol.style.Fill({ color: circle.backgroundColor }),
        stroke: new ol.style.Stroke({ color: circle.borderColor, width: circle.borderSize }),
        text: new ol.style.Text({
            //textAlign: "Start",
            //textBaseline: "Middle",
            overflow: true,
            text: circle.label,
            placement: "line",
            scale: circle.textScale,
            fill: new ol.style.Fill({ color: circle.color }),
            stroke: new ol.style.Stroke({ color: circle.borderColor, width: circle.borderSize }),
            offsetX: 0,
            offsetY: 0,
            rotation: 0
        }),
    });   
}
