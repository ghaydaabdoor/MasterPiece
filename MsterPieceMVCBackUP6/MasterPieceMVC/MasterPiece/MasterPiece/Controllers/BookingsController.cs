using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterPiece.Models;
using MasterPiece.PayPal;
using MasterPiece.ViewModels;

namespace MasterPiece.Controllers
{
    public class BookingsController : Controller
    {
        private MasterPieceEntities5 db = new MasterPieceEntities5();

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        // Wedding Venues
        public ActionResult Create(int? id)
        {
            // Check if the user is logged in by verifying the presence of the "UserId" session variable.
            if (Session["UserId"] == null)
            {
                // If the user is not logged in, return a JSON response to trigger SweetAlert in the view.
                TempData["LoginRequired"] = true; // Set a temporary data flag to indicate login is required.
                return RedirectToAction("Index", "WeddingVenueDetails", new { id = id });
            }

            int userId = (int)Session["UserId"];
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var model = new Booking
                {
                    Email = user.Email,
                    Phone = user.Phone,
                    UserId = userId,
                    VenueId = id,
                    WeddingVenue = db.WeddingVenues.FirstOrDefault(v => v.VenueId == id),
                };
                return View(model);
            }

            return View(new Booking());
        }



        // Wedding Venues
        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookingId,UserId,FullName,Email,Phone,BookingDate,BookingTimeFrom,SpecialOrder,IsApproved,BookingTimeTo,VenueId")] Booking booking)
        {
            if (booking.BookingDate == null)
            {
                // Add error message to the ModelState
                ModelState.AddModelError("BookingDate", "يرجى تحديد تاريخ الحجز");
            }
            // Check for existing bookings at the same venue, date, and overlapping time
            var conflictingBooking = db.Bookings
                .Where(b => b.VenueId == booking.VenueId
                            && b.BookingDate == booking.BookingDate
                            && ((booking.BookingTimeFrom >= b.BookingTimeFrom && booking.BookingTimeFrom < b.BookingTimeTo)
                                || (booking.BookingTimeTo > b.BookingTimeFrom && booking.BookingTimeTo <= b.BookingTimeTo)
                                || (booking.BookingTimeFrom <= b.BookingTimeFrom && booking.BookingTimeTo >= b.BookingTimeTo)))
                .FirstOrDefault();

            if (conflictingBooking != null)
            {
                ModelState.AddModelError("BookingTimeFrom", "هذا الوقت محجوز بالفعل. يرجى اختيار وقت آخر");
                return View(booking);
            }

            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();

                TempData["BookingId"] = booking.BookingId;
                TempData["BookingSuccess"] = "تم حجزك بنجاح";
                TempData.Keep("BookingSuccess");
                return RedirectToAction("Create", "Bookings", new { id = booking.VenueId });  // Redirect to a different page on successful booking.
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", booking.UserId);
            return View(booking);  // Return the view with errors if any.
        }





        //------------------------
        // Farms
        public ActionResult CreateFarmForm(int? id)
        {
            // Check if the user is logged in by verifying the presence of the "UserId" session variable.
            if (Session["UserId"] == null)
            {
                // If the user is not logged in, return a JSON response to trigger SweetAlert in the view.
                TempData["LoginRequired"] = true; // Set a temporary data flag to indicate login is required.
                return RedirectToAction("Index", "FarmDetails", new { id = id });
            }

            int userId = (int)Session["UserId"];

            var user = db.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null)
            {
                var model = new Booking
                {
                    Email = user.Email,
                    Phone = user.Phone,
                    UserId = userId,
                    FarmId = id,
                    Farm = db.Farms.FirstOrDefault(v => v.FarmId == id),
                };
                return View(model);
            }
            return View(new Booking());
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFarmForm([Bind(Include = "BookingId,UserId,FullName,Email,Phone,BookingDate,BookingTimeFrom,SpecialOrder,IsApproved,BookingTimeTo,FarmId")] Booking booking)
        {
            if (booking.BookingDate == null)
            {
                // Add error message to the ModelState
                ModelState.AddModelError("BookingDate", "يرجى تحديد تاريخ الحجز");
            }
            // Check for existing bookings at the same farm, date, and overlapping time
            var conflictingBooking = db.Bookings
                .Where(b => b.FarmId == booking.FarmId
                            && b.BookingDate == booking.BookingDate
                            && ((booking.BookingTimeFrom >= b.BookingTimeFrom && booking.BookingTimeFrom < b.BookingTimeTo)
                                || (booking.BookingTimeTo > b.BookingTimeFrom && booking.BookingTimeTo <= b.BookingTimeTo)
                                || (booking.BookingTimeFrom <= b.BookingTimeFrom && booking.BookingTimeTo >= b.BookingTimeTo)))
                .FirstOrDefault();

            if (conflictingBooking != null)
            {
                ModelState.AddModelError("BookingTimeFrom", "هذا الوقت محجوز بالفعل. يرجى اختيار وقت آخر");
                return View(booking);
            }
            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();

                TempData["BookingId"] = booking.BookingId;
                TempData["BookingSuccess"] = "تم حجزك بنجاح";
                TempData.Keep("BookingSuccess");
                return RedirectToAction("CreateFarmForm", "Bookings", new { id = booking.FarmId });
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", booking.UserId);
            return View(booking);
        }




        //------------------------
        // Photogragphers
        public ActionResult CreatePhotogForm(int? id)
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
                return Json(new { alert = true, message = "خدمة التصوير متاحة فقط بعد إتمام حجز صالة أو مزرعة" }, JsonRequestBehavior.AllowGet);
            }

            if (user != null && hasBookedVenueOrFarm)
            {
                string redirectUrl = Url.Action("CreatePhotogFormView", "Bookings", new { id = id });
                return Json(new { redirect = redirectUrl }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { alert = true, message = "حدث خطأ ما. الرجاء المحاولة لاحقاً." }, JsonRequestBehavior.AllowGet);
        }


        // New Action Method for rendering the CreateClothFormView (this would be the actual page the user goes to).
        public ActionResult CreatePhotogFormView(int id)
        {
            int userId = (int)Session["UserId"];
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);

            var hasBookedVenueOrFarm = db.Bookings.Any(b => b.UserId == userId && (b.VenueId != null || b.FarmId != null));
            System.Diagnostics.Debug.WriteLine($"UserId: {userId}, HasBooked: {hasBookedVenueOrFarm}");

            // Pass the value to the ViewBag
            ViewBag.HasBookedVenueOrFarm = hasBookedVenueOrFarm;

            if (user != null)
            {
                var model = new BookingPhotographer
                {
                    Email = user.Email,
                    Phone = user.Phone,
                    UserId = userId,
                    PhotographerId = (int)id,
                    Photographer = db.Photographers.FirstOrDefault(v => v.PhotographerId == id),
                };
                return View(model);
            }

            return View(new BookingPhotographer());
        }


        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhotogForm([Bind(Include = "BookingPhotogId,UserId,FullName,Email,Phone,BookingDate,BookingTimeFrom,SpecialOrder,IsApproved,BookingTimeTo,PhotographerId")] BookingPhotographer bookingphotog)
        {
            if (bookingphotog.BookingDate == null)
            {
                // Add error message to the ModelState
                ModelState.AddModelError("BookingDate", "يرجى تحديد تاريخ الحجز");
            }
            // Check for existing bookings at the same farm, date, and overlapping time
            var conflictingBooking = db.BookingPhotographers
                .Where(b => b.PhotographerId == bookingphotog.PhotographerId
                            && b.BookingDate == bookingphotog.BookingDate
                            && ((bookingphotog.BookingTimeFrom >= b.BookingTimeFrom && bookingphotog.BookingTimeFrom < b.BookingTimeTo)
                                || (bookingphotog.BookingTimeTo > b.BookingTimeFrom && bookingphotog.BookingTimeTo <= b.BookingTimeTo)
                                || (bookingphotog.BookingTimeFrom <= b.BookingTimeFrom && bookingphotog.BookingTimeTo >= b.BookingTimeTo)))
                .FirstOrDefault();

            if (conflictingBooking != null)
            {
                ModelState.AddModelError("BookingTimeFrom", "هذا الوقت محجوز بالفعل. يرجى اختيار وقت آخر");
                return View(bookingphotog);
            }
            if (ModelState.IsValid)
            {
                db.BookingPhotographers.Add(bookingphotog);
                db.SaveChanges();

                TempData["BookingId"] = bookingphotog.BookingPhotogId;
                TempData["BookingSuccess"] = "تم الحجز بنجاح";
                TempData.Keep("BookingSuccess");
                return RedirectToAction("CreatePhotogForm", "Bookings", new { id = bookingphotog.PhotographerId });
            }

            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", bookingphotog.UserId);
            return View(bookingphotog);
        }






        public ActionResult CreateClothForm(int? id, string returnUrl)
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
                return Json(new { alert = true, message = "خدمة تأجير الملابس متاحة فقط بعد إتمام حجز صالة أو مزرعة" }, JsonRequestBehavior.AllowGet);
            }

            if (user != null && hasBookedVenueOrFarm)
            {
                string redirectUrl = Url.Action("CreateClothFormView", "Bookings", new { id = id });
                return Json(new { redirect = redirectUrl }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { alert = true, message = "حدث خطأ ما. الرجاء المحاولة لاحقاً." }, JsonRequestBehavior.AllowGet);
        }


        // New Action Method for rendering the CreateClothFormView (this would be the actual page the user goes to).
        public ActionResult CreateClothFormView(int id)
        {
            int userId = (int)Session["UserId"];
            var user = db.Users.FirstOrDefault(u => u.UserId == userId);

            var hasBookedVenueOrFarm = db.Bookings.Any(b => b.UserId == userId && (b.VenueId != null || b.FarmId != null));
            System.Diagnostics.Debug.WriteLine($"UserId: {userId}, HasBooked: {hasBookedVenueOrFarm}");

            // Pass the value to the ViewBag
            ViewBag.HasBookedVenueOrFarm = hasBookedVenueOrFarm;

            if (user != null)
            {
                var model = new BookingCloth
                {
                    Email = user.Email,
                    Phone = user.Phone,
                    UserId = userId,
                    ClothId = id,
                    Cloth = db.Clothes.FirstOrDefault(v => v.ClothId == id),
                };
                return View(model);
            }

            return View(new BookingCloth());
        }



        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClothForm([Bind(Include = "BookingClothId,UserId,FullName,Email,Phone,BookingDateFrom,BookingDateTo,SpecialOrder,IsApproved,ClothId")] BookingCloth bookingcloth)
        {
            if (bookingcloth.BookingDateFrom == null || bookingcloth.BookingDateTo == null)
            {
                // Add error message to the ModelState
                ModelState.AddModelError("BookingDateFrom", "يرجى تحديد تاريخ بداية ونهاية الحجز");
                return View("CreateClothFormView", bookingcloth);
            }

            // Check for existing bookings at the same venue, date, and overlapping time
            var conflictingBooking = db.BookingCloths.Where(b => b.ClothId == bookingcloth.ClothId
                 && bookingcloth.BookingDateFrom < b.BookingDateTo
                 && bookingcloth.BookingDateTo > b.BookingDateFrom).FirstOrDefault();

            if (conflictingBooking != null)
            {
                // Add error message to the ModelState
                ModelState.AddModelError("BookingDateFrom", "هذا الوقت محجوز بالفعل. يرجى اختيار وقت آخر");
                bookingcloth.Cloth = db.Clothes.FirstOrDefault(v => v.ClothId == bookingcloth.ClothId);
                return View("CreateClothFormView", bookingcloth);
            }

            if (ModelState.IsValid)
            {
                db.BookingCloths.Add(bookingcloth);
                db.SaveChanges();

                TempData["BookingIdd"] = bookingcloth.BookingClothId;
                TempData["BookingSuccess"] = "تم حجزك بنجاح";
                TempData.Keep("BookingSuccess");

                return RedirectToAction("CreateClothFormView", "Bookings", new { id = bookingcloth.ClothId });
            }

            // Re-populate the ViewBag in case of form errors
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", bookingcloth.UserId);

            return View("CreateClothFormView", bookingcloth);
        }












        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", booking.UserId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,UserId,FullName,Email,Phone,BookingDate,BookingTimeFrom,SpecialOrder,IsApproved,BookingTimeTo")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FirstName", booking.UserId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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







        // POST: Process Payment for Wedding Venue Booking
        [HttpPost]
        public ActionResult ProcessPaymentVenue(int bookingId, int venueId, decimal value)
        {
            // Find the wedding venue booking in the database
            var booking = db.Bookings.FirstOrDefault(b => b.BookingId == bookingId && b.VenueId == venueId);
            if (booking == null)
            {
                return HttpNotFound("Booking not found.");
            }

            // Define the redirect URLs for success and cancellation
            string redirectUrl = Url.Action("PaymentSuccessVenue", "Bookings", new { bookingId, venueId }, protocol: Request.Url.Scheme);
            string cancelUrl = Url.Action("PaymentCancelVenue", "Bookings", new { bookingId, venueId }, protocol: Request.Url.Scheme);

            // Create PayPal payment with the booking amount
            var payment = PayPalHelper.CreatePayment(redirectUrl, cancelUrl, value);

            // Get PayPal redirect URL and redirect the user
            var redirect = payment.links.FirstOrDefault(link => link.rel.ToLower().Trim().Equals("approval_url"));
            if (redirect == null)
            {
                return View("Error");
            }

            return Redirect(redirect.href);
        }

        // GET: Handle the success response from PayPal
        public ActionResult PaymentSuccessVenue(string paymentId, string token, string PayerID, int bookingId, int venueId)
        {
            var executedPayment = PayPalHelper.ExecutePayment(paymentId, PayerID);

            if (executedPayment.state.ToLower() != "approved")
            {
                return View("Error");
            }

            // Optional: Update booking status in the database as paid
            var booking = db.Bookings.FirstOrDefault(b => b.BookingId == bookingId && b.VenueId == venueId);
            if (booking != null)
            {
                booking.IsApproved = true; // Mark the booking as paid or approved (assumes you have an `IsApproved` property)
                db.SaveChanges();
            }

            return View("CloseTab");
        }

        // GET: Handle the cancellation response from PayPal
        public ActionResult PaymentCancelVenue(int bookingId, int venueId)
        {
            return RedirectToAction("Create", "Bookings"); // Redirect to the homepage or booking page
        }

        public ActionResult CloseTab()
        {
            return View();
        }





        // POST: Process Payment for Farm Booking
        [HttpPost]
        public ActionResult ProcessPaymentFarm(int bookingId, int farmId, decimal value)
        {
            // Find the farm booking in the database
            var booking = db.Bookings.FirstOrDefault(b => b.BookingId == bookingId && b.FarmId == farmId);
            if (booking == null)
            {
                return HttpNotFound("Booking not found.");
            }

            // Define the redirect URLs for success and cancellation
            string redirectUrl = Url.Action("PaymentSuccessFarm", "Bookings", new { bookingId, farmId }, protocol: Request.Url.Scheme);
            string cancelUrl = Url.Action("PaymentCancelFarm", "Bookings", new { bookingId, farmId }, protocol: Request.Url.Scheme);

            // Create PayPal payment with the booking amount
            var payment = PayPalHelper.CreatePayment(redirectUrl, cancelUrl, value);

            // Get PayPal redirect URL and redirect the user
            var redirect = payment.links.FirstOrDefault(link => link.rel.ToLower().Trim().Equals("approval_url"));
            if (redirect == null)
            {
                return View("Error");
            }

            return Redirect(redirect.href);
        }

        // GET: Handle the success response from PayPal
        public ActionResult PaymentSuccessFarm(string paymentId, string token, string PayerID, int bookingId, int farmId)
        {
            var executedPayment = PayPalHelper.ExecutePayment(paymentId, PayerID);

            if (executedPayment.state.ToLower() != "approved")
            {
                return View("Error");
            }

            // Optional: Update booking status in the database as paid
            var booking = db.Bookings.FirstOrDefault(b => b.BookingId == bookingId && b.FarmId == farmId);
            if (booking != null)
            {
                booking.IsApproved = true; // Mark the booking as paid or approved (assumes you have an `IsApproved` property)
                db.SaveChanges();
            }

            return View("CloseTab");
        }

        // GET: Handle the cancellation response from PayPal
        public ActionResult PaymentCancelFarm(int bookingId, int farmId)
        {
            return RedirectToAction("CreateFarmForm", "Bookings"); // Redirect to the homepage or booking page
        }





        // POST: Process Payment for Cloth Booking
        [HttpPost]
        public ActionResult ProcessPaymentCloth(int bookingClothId, int clothId, decimal value)
        {
            // Find the cloth booking in the database
            var booking = db.BookingCloths.FirstOrDefault(b => b.BookingClothId == bookingClothId && b.ClothId == clothId);
            if (booking == null)
            {
                return HttpNotFound("Booking not found.");
            }

            // Define the redirect URLs for success and cancellation
            string redirectUrl = Url.Action("PaymentSuccessCloth", "Bookings", new { bookingClothId, clothId }, protocol: Request.Url.Scheme);
            string cancelUrl = Url.Action("PaymentCancelCloth", "Bookings", new { bookingClothId, clothId }, protocol: Request.Url.Scheme);

            // Create PayPal payment with the booking amount
            var payment = PayPalHelper.CreatePayment(redirectUrl, cancelUrl, value);

            // Get PayPal redirect URL and redirect the user
            var redirect = payment.links.FirstOrDefault(link => link.rel.ToLower().Trim().Equals("approval_url"));
            if (redirect == null)
            {
                return View("Error");
            }

            return Redirect(redirect.href);
        }

        // GET: Handle the success response from PayPal
        public ActionResult PaymentSuccessCloth(string paymentId, string token, string PayerID, int bookingClothId, int clothId)
        {
            var executedPayment = PayPalHelper.ExecutePayment(paymentId, PayerID);

            if (executedPayment.state.ToLower() != "approved")
            {
                return View("Error");
            }

            // Optional: Update booking status in the database as paid
            var booking = db.BookingCloths.FirstOrDefault(b => b.BookingClothId == bookingClothId && b.ClothId == clothId);
            if (booking != null)
            {
                booking.IsApproved = true; // Mark the booking as paid or approved (assumes you have an `IsApproved` property)
                db.SaveChanges();
            }

            return View("CloseTab");
        }

        // GET: Handle the cancellation response from PayPal
        public ActionResult PaymentCancelCloth(int bookingClothId, int clothId)
        {
            return RedirectToAction("CreateClothForm", "Bookings");
        }




        // POST: Process Payment for Photographers Booking
        [HttpPost]
        public ActionResult ProcessPaymentPhotog(int BookingPhotogId, int PhotographerId, decimal value)
        {
            // Find the cloth booking in the database
            var booking = db.BookingPhotographers.FirstOrDefault(b => b.BookingPhotogId == BookingPhotogId && b.PhotographerId == PhotographerId);
            if (booking == null)
            {
                return HttpNotFound("Booking not found.");
            }

            // Define the redirect URLs for success and cancellation
            string redirectUrl = Url.Action("PaymentSuccessPhotog", "Bookings", new { BookingPhotogId, PhotographerId }, protocol: Request.Url.Scheme);
            string cancelUrl = Url.Action("PaymentCancelPhotog", "Bookings", new { BookingPhotogId, PhotographerId }, protocol: Request.Url.Scheme);

            // Create PayPal payment with the booking amount
            var payment = PayPalHelper.CreatePayment(redirectUrl, cancelUrl, value);

            // Get PayPal redirect URL and redirect the user
            var redirect = payment.links.FirstOrDefault(link => link.rel.ToLower().Trim().Equals("approval_url"));
            if (redirect == null)
            {
                return View("Error");
            }

            return Redirect(redirect.href);
        }

        // GET: Handle the success response from PayPal
        public ActionResult PaymentSuccessPhotog(string paymentId, string token, string PayerID, int BookingPhotogId, int PhotographerId)
        {
            var executedPayment = PayPalHelper.ExecutePayment(paymentId, PayerID);

            if (executedPayment.state.ToLower() != "approved")
            {
                return View("Error");
            }

            // Optional: Update booking status in the database as paid
            var bookingphotog = db.BookingPhotographers.FirstOrDefault(b => b.BookingPhotogId == BookingPhotogId && b.PhotographerId == PhotographerId);
            if (bookingphotog != null)
            {
                bookingphotog.IsApproved = true; // Mark the booking as paid or approved (assumes you have an `IsApproved` property)
                db.SaveChanges();
            }

            return View("CloseTab");
        }

        // GET: Handle the cancellation response from PayPal
        public ActionResult PaymentCancelPhotog(int BookingPhotogId, int PhotographerId)
        {
            return RedirectToAction("CreatePhotogForm", "Bookings");
        }
    }
}
