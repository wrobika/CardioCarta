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
    
    public partial class Diary
    {
        public string Id { get; set; }
        public string Patient_AspNetUsers_Id { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public int Mood { get; set; }
        public int SystolicPressure { get; set; }
        public int DiastolicPressure { get; set; }
        public bool RespirationProblem { get; set; }
        public bool Haemorrhage { get; set; }
        public bool Dizziness { get; set; }
        public bool ChestPain { get; set; }
        public bool SternumPain { get; set; }
        public bool HeartPain { get; set; }
        public bool Alcohol { get; set; }
        public bool Coffee { get; set; }
        public string Other { get; set; }
    
        public virtual Patient Patient { get; set; }
    }
}
