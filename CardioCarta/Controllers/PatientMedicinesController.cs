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

namespace CardioCarta.Controllers
{
    public class PatientMedicinesController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: PatientMedicines
        [Authorize(Roles = "Patient")]
        public ActionResult Index()
        {
            var patientMedicine = db.PatientMedicine.Include(p => p.Medicine).Include(p => p.Patient).Include(p => p.TakingMedicineTime);
            var id = User.Identity.GetUserId();
            var myMedicine = patientMedicine.Where(p => p.Patient_AspNetUsers_Id == id);
            return View(myMedicine.ToList());
        }

        [Authorize(Roles = "Doctor")]
        public ActionResult IndexForDoctor(string patientId)
        {
            //sprawdzanie czy pacjent jest wsród pacjentów lekarza
            var doctorId = User.Identity.GetUserId();
            var doctor = db.Doctor.SingleOrDefault(d => d.AspNetUsers_Id == doctorId);
            var patient = doctor.Patient.SingleOrDefault(p => p.AspNetUsers_Id == patientId);
            if (patient == null)
            {
                return HttpNotFound();
            }
            var patientMedicine = db.PatientMedicine.Include(p => p.Medicine).Include(p => p.Patient).Include(p => p.TakingMedicineTime);
            var myMedicine = patientMedicine.Where(p => p.Patient_AspNetUsers_Id == patientId);
            return View("Index", myMedicine.ToList());
        }

        // GET: PatientMedicines/Details/5
        public ActionResult Details(string userId, string name, string time)
        {
            if (userId == null || name == null || time == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedicine patientMedicine = db.PatientMedicine.Find(userId, name, time);
            if (patientMedicine == null)
            {
                return HttpNotFound();
            }
            return View(patientMedicine);
        }

        // GET: PatientMedicines/Create
        public ActionResult Create()
        {
            MedicinesController medicinesController = new MedicinesController();
            // ViewBag.Medicine_Name = new SelectList(medicinesController.GetMedicinesList());
            //ViewBag.TakingTime = new SelectList(medicinesController.GetTakingTimeList());

            ViewBag.Medicine_Name = new SelectList(db.Medicine.OrderBy(m => m.Name), "Name", "Name");
            ViewBag.TakingTime = new SelectList(db.TakingMedicineTime, "TakingTime", "TakingTime");
            return View();
        }

        // POST: PatientMedicines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Medicine_Name,MgDose,TakingTime")] PatientMedicine patientMedicine)
        {
            patientMedicine.Patient_AspNetUsers_Id = User.Identity.GetUserId();
            patientMedicine.Id = UniqueId();
            //patientMedicine.Medicine = db.Medicine.Single(m => m.Name == patientMedicine.Medicine_Name);
            //patientMedicine.Patient = db.Patient.Single(p => p.AspNetUsers_Id == patientMedicine.Patient_AspNetUsers_Id);
            //patientMedicine.TakingMedicineTime = db.TakingMedicineTime.Single(t => t.TakingTime == patientMedicine.TakingTime);

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
            {
                db.PatientMedicine.Add(patientMedicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Medicine_Name = new SelectList(db.Medicine, "Name", "Name", patientMedicine.Medicine_Name);
            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", patientMedicine.Patient_AspNetUsers_Id);
            ViewBag.TakingTime = new SelectList(db.TakingMedicineTime, "TakingTime", "TakingTime", patientMedicine.TakingTime);
            return View(patientMedicine);
        }

        // GET: PatientMedicines/Edit/5
        public ActionResult Edit(string userId, string name, string time)
        {
            if (userId == null || name ==null || time == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedicine patientMedicine = db.PatientMedicine.Find(userId, name, time);
            if (patientMedicine == null)
            {
                return HttpNotFound();
            }
            ViewBag.Medicine_Name = new SelectList(db.Medicine, "Name", "Name", patientMedicine.Medicine_Name);
            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", patientMedicine.Patient_AspNetUsers_Id);
            ViewBag.TakingTime = new SelectList(db.TakingMedicineTime, "TakingTime", "TakingTime", patientMedicine.TakingTime);
            return View(patientMedicine);
        }

        // POST: PatientMedicines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Patient_AspNetUsers_Id,Medicine_Name,MgDose,TakingTime")] PatientMedicine patientMedicine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientMedicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Medicine_Name = new SelectList(db.Medicine, "Name", "Name", patientMedicine.Medicine_Name);
            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", patientMedicine.Patient_AspNetUsers_Id);
            ViewBag.TakingTime = new SelectList(db.TakingMedicineTime, "TakingTime", "TakingTime", patientMedicine.TakingTime);
            return View(patientMedicine);
        }

        // GET: PatientMedicines/Delete/5
        public ActionResult Delete(string userId, string name, string time)
        {
            if (userId == null || name == null || time == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientMedicine patientMedicine = db.PatientMedicine.Find(userId, name, time);
            if (patientMedicine == null)
            {
                return HttpNotFound();
            }
            return View(patientMedicine);
        }

        // POST: PatientMedicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string userId, string name, string time)
        {
            PatientMedicine patientMedicine = db.PatientMedicine.Find(userId, name, time);
            db.PatientMedicine.Remove(patientMedicine);
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

        private string UniqueId()
        {
            string id = Guid.NewGuid().ToString();
            //szuka unikalnego id
            while (db.PatientMedicine.Where(medicine => medicine.Id == id).ToList().Any())
            {
                id = Guid.NewGuid().ToString();
            }
            return id;
        }
    }
}
