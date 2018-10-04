using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CardioCarta.Models
{
    public class ValueItem
    {
        public string Name { get; set; }
        public double? Value { get; set; }
    }

    public class Index
    {
        public string Name { get; set; }
        public double? Value { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string Advice { get; set; }
        public string Color { get; set; }
    }

    public class Standard
    {
        public string Name { get; set; }
        public string Pollutant { get; set; }
        public double? Limit { get; set; }
        public double? Percent { get; set; }
    }

    public class Current
    {
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public List<ValueItem> Values { get; set; }
        public List<Index> Indexes { get; set; }
        public List<Standard> Standards { get; set; }
    }

    public class History
    {
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public List<ValueItem> Values { get; set; }
        public List<Index> Indexes { get; set; }
        public List<Standard> Standards { get; set; }
    }

    public class Forecast
    {
        public DateTime FromDateTime { get; set; }
        public DateTime TillDateTime { get; set; }
        public List<ValueItem> Values { get; set; }
        public List<Index> Indexes { get; set; }
        public List<Standard> Standards { get; set; }
    }

    public class Measurements
    {
        public Current Current { get; set; }
        public List<History> History { get; set; }
        public List<Forecast> Forecast { get; set; }
    }
}