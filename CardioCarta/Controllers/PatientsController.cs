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
        public ActionResult Index()
        {
            var patient = db.Patient.Include(p => p.PatientInterview);
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
        public ActionResult DiseaseIndex()
        {
            var id = User.Identity.GetUserId();
            Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
            return View(patient.Disease.ToList());
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
        //private ApplicationUserManager _userManager;

        //public PatientsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //}

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}

        //// GET: Patient/DoctorIndex
        //public ActionResult DoctorIndex()
        //{
        //    var id = User.Identity.GetUserId();
        //    Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
        //    return View(patient.Doctor.ToList());
        //}

        //// GET: Patients/AddDoctor
        //public ActionResult AddDoctor()
        //{
        //    ViewBag.Doctors = new SelectList(UserManager.Users.Where(u => u.Roles.), "Name", "Name");
        //    return View();
        //}

        ////POST: Patients/AddDisease
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddDisease([Bind(Include = "Name")] Disease disease)
        //{
        //    var id = User.Identity.GetUserId();
        //    Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
        //    Disease diseaseFromDb = db.Disease.Single(d => d.Name == disease.Name);
        //    diseaseFromDb.Patient.Add(patient);
        //    db.SaveChanges();
        //    return RedirectToAction("DiseaseIndex");
        //}

        ////GET: Patients/DeleteDisease/Miazdzyca
        //public ActionResult DeleteDisease(string name)
        //{
        //    if (name == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var id = User.Identity.GetUserId();
        //    Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
        //    Disease disease = patient.Disease.SingleOrDefault(d => d.Name == name);
        //    if (disease == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(disease);
        //}

        //// POST: Patients/DeleteDisease/Miazdzyca
        //[HttpPost, ActionName("DeleteDisease")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteDiseaseConfirmed(string name)
        //{
        //    var id = User.Identity.GetUserId();
        //    Patient patient = db.Patient.Single(p => p.AspNetUsers_Id == id);
        //    Disease diseaseFromDb = db.Disease.Single(d => d.Name == name);
        //    diseaseFromDb.Patient.Remove(patient);
        //    db.SaveChanges();
        //    return RedirectToAction("DiseaseIndex");
        //}

    }
}
