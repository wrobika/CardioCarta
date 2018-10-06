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
    public class TakenMedicinesController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: TakenMedicines
        public ActionResult Index()
        {
            List<TakenMedicine> myTodayMedicines = GetMyTodayMedicines();
            return View(myTodayMedicines);
        }

        // GET: TakenMedicines/Take
        public ActionResult Take(string patientMedicineID, DateTime day)
        {
            if (patientMedicineID != null && day != null)
            {
                TakenMedicine takenMedicine = db.TakenMedicine.Find(patientMedicineID, day);
                if (takenMedicine != null)
                {
                    takenMedicine.Taken = !takenMedicine.Taken;
                    db.Entry(takenMedicine).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // GET: TakenMedicines/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TakenMedicine takenMedicine = db.TakenMedicine.Find(id);
            if (takenMedicine == null)
            {
                return HttpNotFound();
            }
            return View(takenMedicine);
        }

        // GET: TakenMedicines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TakenMedicines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Diary_Id,PatientMedicine_Id,Taken")] TakenMedicine takenMedicine)
        {
            if (ModelState.IsValid)
            {
                db.TakenMedicine.Add(takenMedicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(takenMedicine);
        }

        // GET: TakenMedicines/Edit/5
        public ActionResult Edit(string patientMedicineID, DateTime dayId)
        {
            if (patientMedicineID == null || dayId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TakenMedicine takenMedicine = db.TakenMedicine.Find(patientMedicineID, dayId);
            if (takenMedicine == null)
            {
                return HttpNotFound();
            }
            return View(takenMedicine);
        }

        // POST: TakenMedicines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Day,PatientMedicine_Id,Taken")] TakenMedicine takenMedicine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(takenMedicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(takenMedicine);
        }

        // GET: TakenMedicines/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TakenMedicine takenMedicine = db.TakenMedicine.Find(id);
            if (takenMedicine == null)
            {
                return HttpNotFound();
            }
            return View(takenMedicine);
        }

        // POST: TakenMedicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TakenMedicine takenMedicine = db.TakenMedicine.Find(id);
            db.TakenMedicine.Remove(takenMedicine);
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

        private List<TakenMedicine> GetMyTodayMedicines()
        {
            string userId = User.Identity.GetUserId();
            List<string> myMedecineIds = db.PatientMedicine.Where(pm => pm.Patient_AspNetUsers_Id == userId).Select(pm => pm.Id).ToList();
            List<TakenMedicine> myTodayMedicines = new List<TakenMedicine>();
            foreach (string medicineId in myMedecineIds)
            {
                try
                {
                    TakenMedicine takenMedicine = db.TakenMedicine.Where(tm => tm.PatientMedicine_Id == medicineId).Single(tm => tm.Day == DateTime.Today);
                    takenMedicine.PatientMedicine = db.PatientMedicine.Single(pm => pm.Id == medicineId);
                    myTodayMedicines.Add(takenMedicine);
                }
                catch (InvalidOperationException)
                {
                    TakenMedicine takenMedicine = new TakenMedicine()
                    {
                        Day = DateTime.Today,
                        PatientMedicine_Id = medicineId,
                        Taken = false,
                        PatientMedicine = db.PatientMedicine.Single(pm => pm.Id == medicineId),
                    };
                    myTodayMedicines.Add(takenMedicine);
                    db.TakenMedicine.Add(takenMedicine);
                    db.SaveChanges();
                }
            }
            return myTodayMedicines;
        }
    }
}
