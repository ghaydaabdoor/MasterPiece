using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterPiece.Models;

namespace MasterPiece.Controllers
{
    public class PhotographerDetailsController : Controller
    {
        private MasterPieceEntities1 db = new MasterPieceEntities1();

        // GET: PhotographerDetails
        public ActionResult Index(int? id)
        {
            var photographerDetails = db.PhotographerDetails.Include(p => p.Photographer).FirstOrDefault(p=>p.PhotographerId==id);
            return View(photographerDetails);
        }

        // GET: PhotographerDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographerDetail photographerDetail = db.PhotographerDetails.Find(id);
            if (photographerDetail == null)
            {
                return HttpNotFound();
            }
            return View(photographerDetail);
        }

        // GET: PhotographerDetails/Create
        public ActionResult Create()
        {
            ViewBag.PhotographerId = new SelectList(db.Photographers, "PhotographerId", "Name");
            return View();
        }

        // POST: PhotographerDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhotographerDetailId,PhotographerId,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ImageUrl6,PriceDetails,Description")] PhotographerDetail photographerDetail)
        {
            if (ModelState.IsValid)
            {
                db.PhotographerDetails.Add(photographerDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PhotographerId = new SelectList(db.Photographers, "PhotographerId", "Name", photographerDetail.PhotographerId);
            return View(photographerDetail);
        }

        // GET: PhotographerDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographerDetail photographerDetail = db.PhotographerDetails.Find(id);
            if (photographerDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.PhotographerId = new SelectList(db.Photographers, "PhotographerId", "Name", photographerDetail.PhotographerId);
            return View(photographerDetail);
        }

        // POST: PhotographerDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhotographerDetailId,PhotographerId,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ImageUrl6,PriceDetails,Description")] PhotographerDetail photographerDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photographerDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhotographerId = new SelectList(db.Photographers, "PhotographerId", "Name", photographerDetail.PhotographerId);
            return View(photographerDetail);
        }

        // GET: PhotographerDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhotographerDetail photographerDetail = db.PhotographerDetails.Find(id);
            if (photographerDetail == null)
            {
                return HttpNotFound();
            }
            return View(photographerDetail);
        }

        // POST: PhotographerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PhotographerDetail photographerDetail = db.PhotographerDetails.Find(id);
            db.PhotographerDetails.Remove(photographerDetail);
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
