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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Airly()
        {
            this.AirlyForecast = new HashSet<AirlyForecast>();
        }
    
        public string Diary_Id { get; set; }
        public Nullable<float> Airly_CAQI { get; set; }
        public Nullable<float> PM1 { get; set; }
        public Nullable<float> PM10 { get; set; }
        public Nullable<float> PM25 { get; set; }
        public Nullable<float> Humidity { get; set; }
        public Nullable<float> Pressure { get; set; }
        public Nullable<float> Temperature { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AirlyForecast> AirlyForecast { get; set; }
    }
}
