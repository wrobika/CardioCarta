using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CardioCarta.Models;

namespace CardioCarta.Controllers
{
    public class AirliesController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: Airlies
        public ActionResult Index()
        {
            return View(db.Airly.ToList());
        }

        // GET: Airlies/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airly airly = db.Airly.Find(id);
            if (airly == null)
            {
                return HttpNotFound();
            }
            return View(airly);
        }

        // GET: Airlies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Airlies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Diary_Id,Airly_CAQI,PM1,PM10,PM25,Humidity,Pressure,Temperature")] Airly airly)
        {
            if (ModelState.IsValid)
            {
                db.Airly.Add(airly);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(airly);
        }

        // GET: Airlies/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airly airly = db.Airly.Find(id);
            if (airly == null)
            {
                return HttpNotFound();
            }
            return View(airly);
        }

        // POST: Airlies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Diary_Id,Airly_CAQI,PM1,PM10,PM25,Humidity,Pressure,Temperature")] Airly airly)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airly).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airly);
        }

        // GET: Airlies/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airly airly = db.Airly.Find(id);
            if (airly == null)
            {
                return HttpNotFound();
            }
            return View(airly);
        }

        // POST: Airlies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Airly airly = db.Airly.Find(id);
            db.Airly.Remove(airly);
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
