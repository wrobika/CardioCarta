//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CardioCarta.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Airly
    {
        public Nullable<float> Airly_CAQI { get; set; }
        public Nullable<float> PM1 { get; set; }
        public Nullable<float> PM10 { get; set; }
        public Nullable<float> PM25 { get; set; }
        public Nullable<float> Humidity { get; set; }
        public Nullable<float> Pressure { get; set; }
        public Nullable<float> Temperature { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public int SensorId { get; set; }
    
        public virtual AirlySensor AirlySensor { get; set; }
    }
}
