using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardioCarta.Models
{
    public class WarningViewModel
    {
        [Key]
        [Required]
        [Display(Name = "Twoja lokalizacja pobierze się automatycznie")]
        [RegularExpression(@"\d+\.\d+ \d+\.\d+$")]
        public string Location { get; set; }

        [Display(Name = "Temperatura powietrza")]
        [DisplayFormat(DataFormatString = "{0:0.0 °C}")]
        public double? Temperature { get; set; }

        [Display(Name = "Ciśnienie atmosferyczne")]
        [DisplayFormat(DataFormatString = "{0:0 hPa}")]
        public double? Pressure { get; set; }

        [Display(Name = "AirlyCAQI")]
        [DisplayFormat(DataFormatString = "{0:0.0}")]
        public double? AirlyCAQI { get; set; }

        [Display(Name = "Jakość powietrza")]
        public string Quality { get; set; }

        [Display(Name = "Ostrzeżenie")]
        public string Description { get; set; }
    }
}