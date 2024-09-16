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
    public class WeddingVenuesController : Controller
    {
        private MasterPieceEntities1 db = new MasterPieceEntities1();

        // GET: WeddingVenues
        public ActionResult Index()
        {
            return View(db.WeddingVenues.ToList());
        }

        // GET: WeddingVenues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeddingVenue weddingVenue = db.WeddingVenues.Find(id);
            if (weddingVenue == null)
            {
                return HttpNotFound();
            }
            return View(weddingVenue);
        }

        // GET: WeddingVenues/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WeddingVenues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VenueId,Name,Location,Price,Rate,ImageUrl")] WeddingVenue weddingVenue)
        {
            if (ModelState.IsValid)
            {
                db.WeddingVenues.Add(weddingVenue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(weddingVenue);
        }

        // GET: WeddingVenues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeddingVenue weddingVenue = db.WeddingVenues.Find(id);
            if (weddingVenue == null)
            {
                return HttpNotFound();
            }
            return View(weddingVenue);
        }

        // POST: WeddingVenues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VenueId,Name,Location,Price,Rate,ImageUrl")] WeddingVenue weddingVenue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(weddingVenue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(weddingVenue);
        }

        // GET: WeddingVenues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WeddingVenue weddingVenue = db.WeddingVenues.Find(id);
            if (weddingVenue == null)
            {
                return HttpNotFound();
            }
            return View(weddingVenue);
        }

        // POST: WeddingVenues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WeddingVenue weddingVenue = db.WeddingVenues.Find(id);
            db.WeddingVenues.Remove(weddingVenue);
            db.SaveChanges();
            return View("Index");
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
