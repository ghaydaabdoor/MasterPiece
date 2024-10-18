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
            if (Session["UserId"] == null)
            {
                return Json(new { loginRequired = true }, JsonRequestBehavior.AllowGet);
            }

            int userId = (int)Session["UserId"];
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);

            // Check if the user has booked a venue or farm
            var hasBookedVenueOrFarm = db.Bookings.Any(b => b.UserId == userId && (b.VenueId != null || b.FarmId != null));
            System.Diagnostics.Debug.WriteLine($"UserId: {userId}, HasBooked: {hasBookedVenueOrFarm}");

            ViewBag.HasBookedVenueOrFarm = hasBookedVenueOrFarm;

            // Alert if user has not booked
            if (!hasBookedVenueOrFarm)
            {
                return Json(new { alert = true, message = "خدمة تصميم كرت الدعوة متاحة فقط بعد إتمام حجز صالة أو مزرعة" }, JsonRequestBehavior.AllowGet);
            }

            if (user != null && hasBookedVenueOrFarm)
            {
                string redirectUrl = Url.Action("CreateInvitationCardFormView", "InvitationCardForms", new { id = id });
                return Json(new { redirect = redirectUrl }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { alert = true, message = "حدث خطأ ما. الرجاء المحاولة لاحقاً." }, JsonRequestBehavior.AllowGet);
        }

        // New Action Method for rendering the CreateClothFormView (this would be the actual page the user goes to).
        public ActionResult CreateInvitationCardFormView(int id)
        {
            int userId = (int)Session["UserId"];
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);

            var hasBookedVenueOrFarm = db.Bookings.Any(b => b.UserId == userId && (b.VenueId != null || b.FarmId != null));
            System.Diagnostics.Debug.WriteLine($"ViewBag.HasBookedVenueOrFarm: {ViewBag.HasBookedVenueOrFarm}");

            // Pass the value to the ViewBag
            ViewBag.HasBookedVenueOrFarm = hasBookedVenueOrFarm;


            if (user != null)
            {
                InvitationCardForm form = db.InvitationCardForms.FirstOrDefault(p => p.CardDesignId == id);
                return View(form);
            }

            return View(new InvitationCardForm());
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
                return RedirectToAction("CreateInvitationCardFormView", new {id=form.CardDesignId});
            }
            return View(form);
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
