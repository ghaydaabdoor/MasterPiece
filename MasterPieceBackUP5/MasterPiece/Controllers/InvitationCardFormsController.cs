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
    public class InvitationCardFormsController : Controller
    {
        private MasterPieceEntities5 db = new MasterPieceEntities5();

        // GET: InvitationCardForms
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvitationCardForm form = db.InvitationCardForms.FirstOrDefault(p=>p.CardDesignId == id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvitationCardFormId,CardDesignId,GroomName,BrideName,WeddingDate,Venue,AdditionalNotes,Status")] InvitationCardForm form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessCardMessage"] = "تم ارسال الطلب سوف يتم ارساله عبر البريد الالكتروني قريباً";
                return RedirectToAction("Index", new {id=form.CardDesignId});
            }
            return View(form);
        }






       

        // GET: InvitationCardForms/Create
        public ActionResult Create()
        {
            ViewBag.CardDesignId = new SelectList(db.InvitationCards, "InvitationCardsId", "CardName");
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Summary");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName");
            return View();
        }

        // POST: InvitationCardForms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvitationCardFormId,OrderId,UserId,CardDesignId,GroomName,BrideName,WeddingDate,Venue,AdditionalNotes,Status")] InvitationCardForm invitationCardForm)
        {
            if (ModelState.IsValid)
            {
                db.InvitationCardForms.Add(invitationCardForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CardDesignId = new SelectList(db.InvitationCards, "InvitationCardsId", "CardName", invitationCardForm.CardDesignId);
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Summary", invitationCardForm.OrderId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", invitationCardForm.UserId);
            return View(invitationCardForm);
        }

        // GET: InvitationCardForms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvitationCardForm invitationCardForm = db.InvitationCardForms.Find(id);
            if (invitationCardForm == null)
            {
                return HttpNotFound();
            }
            return View(invitationCardForm);
        }

        // POST: InvitationCardForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvitationCardForm invitationCardForm = db.InvitationCardForms.Find(id);
            db.InvitationCardForms.Remove(invitationCardForm);
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
