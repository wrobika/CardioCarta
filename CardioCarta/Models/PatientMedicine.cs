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
    using System.ComponentModel.DataAnnotations;

    public partial class PatientMedicine
    {
        public string Id { get; set; }

        public string Patient_AspNetUsers_Id { get; set; }

        [Required]
        [Display(Name = "Nazwa leku")]
        public string Medicine_Name { get; set; }

        [Required]
        [Range(0,5000)]
        [Display(Name = "Dawka w miligramach")]
        public int MgDose { get; set; }

        [Required]
        [Display(Name = "Pora przyjmowania")]
        public string TakingTime { get; set; }
    
        public virtual Medicine Medicine { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual TakingMedicineTime TakingMedicineTime { get; set; }
    }
}
