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
    public class FoodDetailsController : Controller
    {
        private MasterPieceEntities1 db = new MasterPieceEntities1();

        // GET: FoodDetails
        public ActionResult Index(int? id)
        {
            var foodID = db.Foods.Find(id);
            var foodDetailsID = db.FoodDetails.FirstOrDefault(p => p.FoodId == id);

            var viewModel = new FoodViewModel
            {
                food = foodID,
                foodDetail = foodDetailsID,
            };

            return View(viewModel);
        }


        // GET: FoodDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodDetail foodDetail = db.FoodDetails.Find(id);
            if (foodDetail == null)
            {
                return HttpNotFound();
            }
            return View(foodDetail);
        }

        // GET: FoodDetails/Create
        public ActionResult Create()
        {
            ViewBag.FoodId = new SelectList(db.Foods, "FoodId", "Name");
            return View();
        }

        // POST: FoodDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodDetailId,FoodId,ImageUrl1,ImageUrl2,ImageUrl3,Description,PriceDetails")] FoodDetail foodDetail)
        {
            if (ModelState.IsValid)
            {
                db.FoodDetails.Add(foodDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FoodId = new SelectList(db.Foods, "FoodId", "Name", foodDetail.FoodId);
            return View(foodDetail);
        }

        // GET: FoodDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodDetail foodDetail = db.FoodDetails.Find(id);
            if (foodDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodId = new SelectList(db.Foods, "FoodId", "Name", foodDetail.FoodId);
            return View(foodDetail);
        }

        // POST: FoodDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodDetailId,FoodId,ImageUrl1,ImageUrl2,ImageUrl3,Description,PriceDetails")] FoodDetail foodDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foodDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FoodId = new SelectList(db.Foods, "FoodId", "Name", foodDetail.FoodId);
            return View(foodDetail);
        }

        // GET: FoodDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodDetail foodDetail = db.FoodDetails.Find(id);
            if (foodDetail == null)
            {
                return HttpNotFound();
            }
            return View(foodDetail);
        }

        // POST: FoodDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FoodDetail foodDetail = db.FoodDetails.Find(id);
            db.FoodDetails.Remove(foodDetail);
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
