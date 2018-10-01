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

namespace CardioCarta.Controllers
{
    public class DiariesController : Controller
    {
        private CardioCartaEntities db = new CardioCartaEntities();

        // GET: Diaries
        public ActionResult Index()
        {
            var diary = db.Diary.Include(d => d.Patient);
            return View(diary.ToList());
        }

        // GET: Diaries/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diary diary = db.Diary.Find(id);
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Mood,SystolicPressure,DiastolicPressure,RespirationProblem,Haemorrhage,Dizziness,ChestPain,SternumPain,HeartPain,Alcohol,Coffee,Other")] Diary diary)
        {
            string[] coord = diary.Id.Split(separator: ',');
            diary.Patient_AspNetUsers_Id = User.Identity.GetUserId();
            diary.TimeStamp = DateTime.Now;
            diary.Id = UniqueId();
            if (ModelState.IsValid)
            {
                db.Diary.Add(diary);
                db.SaveChanges();
                await AirlyApi.GetRequest(coord, diary.Id);
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
            Diary diary = db.Diary.Find(id);
            if (diary == null)
            {
                return HttpNotFound();
            }
            ViewBag.Patient_AspNetUsers_Id = new SelectList(db.Patient, "AspNetUsers_Id", "AspNetUsers_Id", diary.Patient_AspNetUsers_Id);
            return View(diary);
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
            Diary diary = db.Diary.Find(id);
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
            Diary diary = db.Diary.Find(id);
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
            while(db.Diary.Where(diary => diary.Id == id).ToList().Any())
            {
                id = Guid.NewGuid().ToString();
            }
            return id;
        }
    }
}
