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
using System.Threading.Tasks;
using NetTopologySuite.Geometries;
using Npgsql;
using GeoAPI.Geometries;

namespace CardioCarta.Controllers
{
    public class DiariesController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: Diaries
        public ActionResult Index()
        {
            //var diary = db.Diary.Include(d => d.Patient);
            var diary = db.Diary;
            var id = User.Identity.GetUserId();
            var userDiaries = diary.Where(d => d.Patient_AspNetUsers_Id == id);
            return View(userDiaries.ToList());
        }

        // GET: Diaries/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diary diary = db.Diary.SingleOrDefault(d => d.Id == id);
            if (diary == null)
            {
                return HttpNotFound();
            }
            return View(diary);
        }

        // GET: Diaries/Filled
        public ActionResult Filled()
        {
            return View();
        }

        // GET: Diaries/Create
        [Authorize(Roles = "Patient")]
        public ActionResult Create()
        {
            //zabepieczenie przed kilkukrotnym wypelnianiem wciagu jednej godziny
            //
            //string userId = User.Identity.GetUserId();
            //if (db.Diary.Where(diary => diary.Patient_AspNetUsers_Id == userId)
            //    .Where(diary => diary.TimeStamp.Hour == DateTime.Now.Hour).Any())
            //{
            //    return RedirectToAction("Filled");
            //}
            return View();
        }

        // POST: Diaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient")]
        public ActionResult Create([Bind(Include = "Id,Mood,SystolicPressure,DiastolicPressure,RespirationProblem,Haemorrhage,Dizziness,ChestPain,SternumPain,HeartPain,Alcohol,Coffee,Other")] Diary diary)
        {
            string coord = diary.Id;
            diary.Patient_AspNetUsers_Id = User.Identity.GetUserId();
            diary.TimeStamp = DateTime.Now;
            diary.Id = UniqueId();
            if (ModelState.IsValid)
            {
                db.Diary.Add(diary);
                db.SaveChanges();
                if (coord != null)
                {
                    SetGeolocation(diary, coord);
                }
                return RedirectToAction("Index");
            }

            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", diary.Patient_AspNetUsers_Id);
            return View(diary);
        }

        // GET: Diaries/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diary diary = db.Diary.SingleOrDefault(d => d.Id == id);
            if (diary == null)
            {
                return HttpNotFound();
            }
            if(diary.TimeStamp <= DateTime.Now.AddHours(-6))
            {
                return RedirectToAction("CannotEdit");
            }
            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", diary.Patient_AspNetUsers_Id);
            return View(diary);
        }

        public ActionResult CannotEdit()
        {
            return View();
        }

        // POST: Diaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Patient_AspNetUsers_Id,TimeStamp,Mood,SystolicPressure,DiastolicPressure,RespirationProblem,Haemorrhage,Dizziness,ChestPain,SternumPain,HeartPain,Alcohol,Coffee,Other")] Diary diary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", diary.Patient_AspNetUsers_Id);
            return View(diary);
        }

        // GET: Diaries/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diary diary = db.Diary.SingleOrDefault(d => d.Id == id);
            if (diary == null)
            {
                return HttpNotFound();
            }
            return View(diary);
        }

        // POST: Diaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Diary diary = db.Diary.SingleOrDefault(d => d.Id == id);
            db.Diary.Remove(diary);
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
            while (db.Diary.Where(diary => diary.Id == id).ToList().Any())
            {
                id = Guid.NewGuid().ToString();
            }
            return id;
        }


        private static void SetGeolocation(Diary diary, string coord)
        {
            NpgsqlConnection connection = new NpgsqlConnection(
                System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            connection.Open();
            NpgsqlCommand command = new NpgsqlCommand(
                "UPDATE \"Diary\" " +
                "SET \"Location\" = ST_PointFromText('POINT(" + coord + ")', 4326) " +
                "WHERE \"Diary\".\"Id\" LIKE '" + diary.Id + "';", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}
