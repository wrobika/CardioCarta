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
    
    public partial class AirlyForecast
    {
        public string Diary_Id { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<float> Airly_CAQI { get; set; }
        public Nullable<float> PM10 { get; set; }
        public Nullable<float> PM25 { get; set; }
    
        public virtual Airly Airly { get; set; }
    }
}
