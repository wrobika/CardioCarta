using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardioCarta.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CardioCarta.Controllers
{
    public class PatientsController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: Patients
        [Authorize(Roles = "Doctor")]
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            Doctor doctor = db.Doctor.Single(p => p.AspNetUsers_Id == id);
            var users = userContext.Users.AsEnumerable();
            var doctorsPatient = doctor.Patient;
            var patient = users.Join(doctorsPatient,
                u => u.Id,
                p => p.AspNetUsers_Id,
                (u, p) => new PatientViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    Surname = u.Surname,
                    CityOrVillage = u.CityOrVillage,
                    District = u.District,
                    Street = u.Street,
                    House = u.House,
                    Flat = u.Flat,
                    PostalCode = u.PostalCode,
                    BirthDate = p.BirthDate,
                    Male = p.Male

                });
            //var patient = db.Patient.Include(p => p.PatientInterview);
            return View(patient.ToList());
        }

        // GET: Patients/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patient.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            ViewBag.AspNetUsers_Id = new SelectList(db.PatientInterview, "Patient_AspNetUsers_Id", "Patient_AspNetUsers_Id");
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AspNetUsers_Id,BirthDate,Male")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patient.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AspNetUsers_Id = new SelectList(db.PatientInterview, "Patient_AspNetUsers_Id", "Patient_AspNetUsers_Id", patient.AspNetUsers_Id);
            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patient.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.AspNetUsers_Id = new SelectList(db.PatientInterview, "Patient_AspNetUsers_Id", "Patient_AspNetUsers_Id", patient.AspNetUsers_Id);
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AspNetUsers_Id,BirthDate,Male")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AspNetUsers_Id = new SelectList(db.PatientInterview, "Patient_AspNetUsers_Id", "Patient_AspNetUsers_Id", patient.AspNetUsers_Id);
            return View(patient);
        }

        // GET: Patients/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patient.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Patient patient = db.Patient.Find(id);
            db.Patient.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //================================ DISEASES =======================================

        // GET: Patient/DiseaseIndex
        [Authorize(Roles = "Patient")]
        public ActionResult DiseaseIndex()
        {
            var id = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
            return View(patient.Disease.ToList());
        }

        [Authorize(Roles = "Doctor")]
        public ActionResult DiseaseIndexForDoctor(string patientId)
        {
            //sprawdzanie czy pacjent jest wsród pacjentów lekarza
            var doctorId = User.Identity.GetUserId();
            var doctor = db.Doctor.SingleOrDefault(d => d.AspNetUsers_Id == doctorId);
            var patient = doctor.Patient.SingleOrDefault(p => p.AspNetUsers_Id == patientId);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View("DiseaseIndex", patient.Disease.ToList());
        }

        // GET: Patients/AddDisease
        public ActionResult AddDisease()
        {
            ViewBag.Diseases = new SelectList(db.Disease, "Name", "Name");
            return View();
        }

        //POST: Patients/AddDisease
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDisease([Bind(Include = "Name")] Disease disease)
        {
            var id = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
            Disease diseaseFromDb = db.Disease.Single(d => d.Name == disease.Name);
            diseaseFromDb.Patient.Add(patient);
            db.SaveChanges();
            return RedirectToAction("DiseaseIndex");
        }

        //GET: Patients/DeleteDisease/Miazdzyca
        public ActionResult DeleteDisease(string name)
        {
            if (name == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var id = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
            Disease disease = patient.Disease.SingleOrDefault(d => d.Name == name);
            if (disease == null)
            {
                return HttpNotFound();
            }
            return View(disease);
        }

        // POST: Patients/DeleteDisease/Miazdzyca
        [HttpPost, ActionName("DeleteDisease")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDiseaseConfirmed(string name)
        {
            var id = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
            Disease diseaseFromDb = db.Disease.Single(d => d.Name == name);
            diseaseFromDb.Patient.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("DiseaseIndex");
        }



        //================================ DOCTORS =======================================
        private ApplicationDbContext userContext = new ApplicationDbContext();

        // GET: Patient/DoctorIndex
        public ActionResult DoctorIndex()
        {
            var id = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
            var users = userContext.Users.AsEnumerable();
            var patientDoctors = patient.Doctor;
            var doctors = users.Join(patientDoctors,
                u => u.Id,
                d => d.AspNetUsers_Id,
                (u, d) => new DoctorViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    Surname = u.Surname,
                    CityOrVillage = u.CityOrVillage,
                    Speciality = d.Speciality_Name
                });
            return View(doctors.ToList());
        }

        // GET: Patients/AddDoctor
        public ActionResult AddDoctor()
        {
            var users = userContext.Users.AsEnumerable();
            var doctors = db.Doctor.AsEnumerable();

            IEnumerable<SelectListItem> doctorsUsers = users.Join(doctors,
                u => u.Id,
                d => d.AspNetUsers_Id,
                (u, d) => new SelectListItem
                {
                    Value = d.AspNetUsers_Id,
                    Text = u.FirstName + " " + u.Surname + ", " + u.CityOrVillage + ", " + d.Speciality_Name
                });

            ViewBag.Doctors = doctorsUsers;
            return View();
        }

        //POST: Patients/AddDoctor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDoctor([Bind(Include = "AspNetUsers_Id")] Doctor doctor)
        {
            var id = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
            Doctor doctorFromDb = db.Doctor.Single(d => d.AspNetUsers_Id == doctor.AspNetUsers_Id);
            doctorFromDb.Patient.Add(patient);
            db.SaveChanges();
            return RedirectToAction("DoctorIndex");
        }

        //GET: Patients/DeleteDocotr/5
        public ActionResult DeleteDoctor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == userId);
            var users = userContext.Users.AsEnumerable();
            var patientDoctors = patient.Doctor;
            var doctors = users.Join(patientDoctors,
                u => u.Id,
                d => d.AspNetUsers_Id,
                (u, d) => new DoctorViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    Surname = u.Surname,
                    CityOrVillage = u.CityOrVillage,
                    Speciality = d.Speciality_Name
                });
            DoctorViewModel doctor = doctors.SingleOrDefault(d => d.Id == id);
            if (doctor == null)
            {
                return HttpNotFound();
            }

            return View(doctor);
        }

        // POST: Patients/DeleteDoctor/5
        [HttpPost, ActionName("DeleteDoctor")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDoctorConfirmed(string id)
        {
            var userId = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == userId);
            Doctor doctorFromDb = db.Doctor.Single(d => d.AspNetUsers_Id == id);
            doctorFromDb.Patient.Remove(patient);
            db.SaveChanges();
            return RedirectToAction("DoctorIndex");
        }
    }
}
