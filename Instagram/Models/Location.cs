﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Location
    {
        public Location(double id)
        {
            this.Id = id;
        }

        public Location(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public Location(double latitude, double longitude, string name, double id)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Name = name;
            this.Id = id;
        }

        public string Name { get; private set; }
        public double Id { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public override string ToString()
        {
            return this.Latitude + ", " + this.Longitude;
        }
    }
}
