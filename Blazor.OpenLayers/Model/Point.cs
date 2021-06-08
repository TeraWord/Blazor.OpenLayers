using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeraWord.Blazor.OpenLayers
{
    public class Point
    {
        public Point() { }

        public Point(params double[] coordinates)
        {
            if (coordinates.Length > 0) Latitude = coordinates[0];
            if (coordinates.Length > 1) Longitude = coordinates[1];
        }

        public Point(Point point)
        {
            Latitude = point.Latitude;
            Longitude = point.Longitude;
        }

        public double Latitude { get => _coordinates[1]; set => _coordinates[1] = value; }

        public double Longitude { get => _coordinates[0]; set => _coordinates[0] = value; }

        /// <summary>
        /// Coordinate in OpenLayers Style: [Longitude, Latitude]
        /// </summary>
        public double[] Coordinates { get => _coordinates; set => _coordinates = value; }
        private double[] _coordinates = new double[2] { 0, 0 };
    }
}
