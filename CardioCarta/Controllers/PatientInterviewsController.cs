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
    public class PatientInterviewsController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: PatientInterviews
        public ActionResult Index()
        {
            var patientInterview = db.PatientInterview.Include(p => p.Patient);
            return View(patientInterview.ToList());
        }

        // GET: PatientInterviews/Filled
        public ActionResult Filled()
        {
            return View();
        }

        // GET: PatientInterviews/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //sprawdzenie czy pacjent jest wsród pacjentów lekarza
            if (User.IsInRole("Doctor"))
            {
                var doctorId = User.Identity.GetUserId();
                var doctor = db.Doctor.SingleOrDefault(d => d.AspNetUsers_Id == doctorId);
                var patient = doctor.Patient.SingleOrDefault(p => p.AspNetUsers_Id == id);
                if (patient == null)
                {
                    return HttpNotFound();
                }
            }

            PatientInterview patientInterview = db.PatientInterview.Find(id);
            if (patientInterview == null)
            {
                return HttpNotFound();
            }
            return View(patientInterview);
        }

        // GET: PatientInterviews/Create
        [Authorize(Roles = "Patient")]
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            if (db.PatientInterview
                .Where(interview => interview.Patient_AspNetUsers_Id == userId).Any())
            {
                return RedirectToAction("Filled");
            }
            return View();
        }

        // POST: PatientInterviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient")]
        public ActionResult Create([Bind(Include = "DiastolicPressure,SystolicPressure,Surgery,DiseaseInFamily,Smoking,Health")] PatientInterview patientInterview)
        {
            patientInterview.Patient_AspNetUsers_Id = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.PatientInterview.Add(patientInterview);
                db.SaveChanges();
                return RedirectToAction("Create", "Diaries");
            }

            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", patientInterview.Patient_AspNetUsers_Id);
            return View(patientInterview);
        }

        // GET: PatientInterviews/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientInterview patientInterview = db.PatientInterview.Find(id);
            if (patientInterview == null)
            {
                return HttpNotFound();
            }
            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", patientInterview.Patient_AspNetUsers_Id);
            return View(patientInterview);
        }

        // POST: PatientInterviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Patient_AspNetUsers_Id,DiastolicPressure,SystolicPressure,Surgery,DiseaseInFamily,Smoking,Health")] PatientInterview patientInterview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patientInterview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", patientInterview.Patient_AspNetUsers_Id);
            return View(patientInterview);
        }

        // GET: PatientInterviews/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PatientInterview patientInterview = db.PatientInterview.Find(id);
            if (patientInterview == null)
            {
                return HttpNotFound();
            }
            return View(patientInterview);
        }

        // POST: PatientInterviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PatientInterview patientInterview = db.PatientInterview.Find(id);
            db.PatientInterview.Remove(patientInterview);
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
    }
}
