using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardioCarta.Models
{
    public class DoctorViewModel
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Display(Name = "Miejscowość")]
        public string CityOrVillage { get; set; }

        [Display(Name = "Specializacja")]
        public string Speciality { get; set; }
    }
}