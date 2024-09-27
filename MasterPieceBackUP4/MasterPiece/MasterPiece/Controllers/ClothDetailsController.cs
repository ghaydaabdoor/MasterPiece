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
    public class ClothDetailsController : Controller
    {
        private MasterPieceEntities4 db = new MasterPieceEntities4();

        // GET: ClothDetails
        public ActionResult Index(int? id)
        {
            var clothDetails = db.ClothDetails.FirstOrDefault(p=>p.ClothId==id);
            return View(clothDetails);
        }

        // GET: ClothDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClothDetail clothDetail = db.ClothDetails.Find(id);
            if (clothDetail == null)
            {
                return HttpNotFound();
            }
            return View(clothDetail);
        }

        // GET: ClothDetails/Create
        public ActionResult Create()
        {
            ViewBag.ClothId = new SelectList(db.Clothes, "ClothId", "Name");
            return View();
        }

        // POST: ClothDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClothDetailId,ClothId,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,Color,Description,ClothMaterial,Features,PriceDetails")] ClothDetail clothDetail)
        {
            if (ModelState.IsValid)
            {
                db.ClothDetails.Add(clothDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClothId = new SelectList(db.Clothes, "ClothId", "Name", clothDetail.ClothId);
            return View(clothDetail);
        }

        // GET: ClothDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClothDetail clothDetail = db.ClothDetails.Find(id);
            if (clothDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClothId = new SelectList(db.Clothes, "ClothId", "Name", clothDetail.ClothId);
            return View(clothDetail);
        }

        // POST: ClothDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClothDetailId,ClothId,ImageUrl1,ImageUrl2,ImageUrl3,ImageUrl4,Color,Description,ClothMaterial,Features,PriceDetails")] ClothDetail clothDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clothDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClothId = new SelectList(db.Clothes, "ClothId", "Name", clothDetail.ClothId);
            return View(clothDetail);
        }

        // GET: ClothDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClothDetail clothDetail = db.ClothDetails.Find(id);
            if (clothDetail == null)
            {
                return HttpNotFound();
            }
            return View(clothDetail);
        }

        // POST: ClothDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClothDetail clothDetail = db.ClothDetails.Find(id);
            db.ClothDetails.Remove(clothDetail);
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
