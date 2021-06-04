using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class GeoPoint
    {
        public GeoPoint() { }

        public GeoPoint(params double[] coordinates)
        {
            if (coordinates.Length > 0) Latitude = coordinates[0];
            if (coordinates.Length > 1) Longitude = coordinates[1];
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double[] AsOpenLayers => new double[2] { Longitude, Latitude };
    }
}
