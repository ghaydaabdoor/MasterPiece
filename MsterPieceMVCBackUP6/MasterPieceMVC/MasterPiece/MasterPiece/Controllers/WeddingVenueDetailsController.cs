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
    public class WeddingVenueDetailsController : Controller
    {
        private MasterPieceEntities5 db = new MasterPieceEntities5();

        // GET: WeddingVenueDetails
        public ActionResult Index(int? id)
        {
            var weddingVenueDetails = db.WeddingVenueDetails.FirstOrDefault(p=>p.VenueId == id);
            return View(weddingVenueDetails);
        }

        // GET: WeddingVenueDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeddingVenueDetail weddingVenueDetail = db.WeddingVenueDetails.Find(id);
            if (weddingVenueDetail == null)
            {
                return HttpNotFound();
            }
            return View(weddingVenueDetail);
        }

        // GET: WeddingVenueDetails/Create
        public ActionResult Create()
        {
            ViewBag.VenueId = new SelectList(db.WeddingVenues, "VenueId", "Name");
            return View();
        }

        // POST: WeddingVenueDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VenueDetailId,VenueId,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ImageUrl6,Description,Services,PriceDetails,Capacity,CapacityNotes,Location,Price")] WeddingVenueDetail weddingVenueDetail)
        {
            if (ModelState.IsValid)
            {
                db.WeddingVenueDetails.Add(weddingVenueDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VenueId = new SelectList(db.WeddingVenues, "VenueId", "Name", weddingVenueDetail.VenueId);
            return View(weddingVenueDetail);
        }

        // GET: WeddingVenueDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeddingVenueDetail weddingVenueDetail = db.WeddingVenueDetails.Find(id);
            if (weddingVenueDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.VenueId = new SelectList(db.WeddingVenues, "VenueId", "Name", weddingVenueDetail.VenueId);
            return View(weddingVenueDetail);
        }

        // POST: WeddingVenueDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VenueDetailId,VenueId,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,ImageUrl5,ImageUrl6,Description,Services,PriceDetails,Capacity,CapacityNotes,Location,Price")] WeddingVenueDetail weddingVenueDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(weddingVenueDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VenueId = new SelectList(db.WeddingVenues, "VenueId", "Name", weddingVenueDetail.VenueId);
            return View(weddingVenueDetail);
        }

        // GET: WeddingVenueDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeddingVenueDetail weddingVenueDetail = db.WeddingVenueDetails.Find(id);
            if (weddingVenueDetail == null)
            {
                return HttpNotFound();
            }
            return View(weddingVenueDetail);
        }

        // POST: WeddingVenueDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WeddingVenueDetail weddingVenueDetail = db.WeddingVenueDetails.Find(id);
            db.WeddingVenueDetails.Remove(weddingVenueDetail);
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
