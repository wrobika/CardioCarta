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
    public class AirlyForecastsController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: AirlyForecasts
        public ActionResult Index()
        {
            return View(db.AirlyForecast.ToList());
        }

        // GET: AirlyForecasts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirlyForecast airlyForecast = db.AirlyForecast.Find(id);
            if (airlyForecast == null)
            {
                return HttpNotFound();
            }
            return View(airlyForecast);
        }

        // GET: AirlyForecasts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AirlyForecasts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TimeStamp,Airly_CAQI,PM10,PM25")] AirlyForecast airlyForecast)
        {
            if (ModelState.IsValid)
            {
                db.AirlyForecast.Add(airlyForecast);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airlyForecast);
        }

        // GET: AirlyForecasts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirlyForecast airlyForecast = db.AirlyForecast.Find(id);
            if (airlyForecast == null)
            {
                return HttpNotFound();
            }
            return View(airlyForecast);
        }

        // POST: AirlyForecasts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TimeStamp,Airly_CAQI,PM10,PM25")] AirlyForecast airlyForecast)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airlyForecast).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airlyForecast);
        }

        // GET: AirlyForecasts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirlyForecast airlyForecast = db.AirlyForecast.Find(id);
            if (airlyForecast == null)
            {
                return HttpNotFound();
            }
            return View(airlyForecast);
        }

        // POST: AirlyForecasts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AirlyForecast airlyForecast = db.AirlyForecast.Find(id);
            db.AirlyForecast.Remove(airlyForecast);
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
