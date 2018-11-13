using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CardioCarta.Models
{
    public class PatientViewModel
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Display(Name = "Miejscowość")]
        public string CityOrVillage { get; set; }

        [Display(Name = "Powiat")]
        public string District { get; set; }

        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Display(Name = "Numer domu")]
        public string House { get; set; }

        [Display(Name = "Numer lokalu")]
        public string Flat { get; set; }

        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Display(Name = "Data urodzenia")]
        public System.DateTime BirthDate { get; set; }

        [Display(Name = "Płeć")]
        public bool Male { get; set; }
    }
}