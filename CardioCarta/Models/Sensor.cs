using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardioCarta.Models
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string DisplayAddress1 { get; set; }
        public string DisplayAddress2 { get; set; }
    }

    public class Sponsor
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public string Link { get; set; }
    }

    public class Sensor
    {
        public int Id { get; set; }
        public Location Location { get; set; }
        public Address Address { get; set; }
        public double Elevation { get; set; }
        public bool Airly { get; set; }
        public Sponsor Sponsor { get; set; }
    }
}