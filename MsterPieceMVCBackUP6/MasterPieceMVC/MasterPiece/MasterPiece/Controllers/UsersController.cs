using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MasterPiece.Models;
using MasterPiece.ViewModels;

namespace MasterPiece.Controllers
{
    public class UsersController : Controller
    {
        private MasterPieceEntities5 db = new MasterPieceEntities5();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Email,Password")] User user)
        {
            if (db.Users.Any(p => p.Email == user.Email))
            {
                ModelState.AddModelError("Email", "البريد الإلكتروني موجود بالفعل. يرجى استخدام بريد إلكتروني آخر");
            }

            if (ModelState.IsValid)
            {
                // Set userRole to "user" before saving
                user.userRole = "user";

                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }


        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl; // Pass the returnUrl to the view
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model, string returnUrl)
        {
            var user = db.Users.FirstOrDefault(p => p.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "البريد الالكتروني غير صحيح");
            }
            else if (model.Password != user.Password)
            {
                ModelState.AddModelError("Password", "كلمة السر غير صحيحة");
            }

            if (ModelState.IsValid)
            {
                Session.Clear();
                Session["UserId"] = user.UserId;

                // Redirect to the original page the user was trying to access before login
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                // If no returnUrl, redirect based on user role
                if (user.userRole == "admin")
                {
                    return RedirectToAction("ShowVenues", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            // If validation fails, return the user to the login page with the original returnUrl
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }



        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Profile()
        {
            var userId = Session["UserId"];
            if (userId == null)
            {
                return RedirectToAction("Login", "Users");
            }

            var user = db.Users.FirstOrDefault(p => p.UserId == (int)userId);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Map the user and other related entities to the ViewModel
            var viewModel = new UserInfoAndServicesViewModel
            {
                user = user,
                booking = db.Bookings.Where(b => b.UserId == (int)userId).ToList(), 
                bookCloth = db.BookingCloths.Where(b => b.UserId == (int)userId).ToList(),
                bookPhotographer = db.BookingPhotographers.Where(b => b.UserId == (int)userId).ToList(),
                invitationCardForm = db.InvitationCardForms.Where(b => b.UserId == (int)userId).ToList(),
            };

            return View(viewModel);  // Pass the ViewModel to the view
        }


        public ActionResult EditProfile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile([Bind(Include = "UserId,FirstName,LastName,Phone,Address")] User model)
        {
            if (ModelState.IsValid)
            {
                // Fetch the existing user to preserve fields not in the form (e.g., Email, Password)
                var existingUser = db.Users.Find(model.UserId);
                if (existingUser != null)
                {
                    // Update only the editable fields
                    existingUser.FirstName = model.FirstName;
                    existingUser.LastName = model.LastName;
                    existingUser.Phone = model.Phone;
                    existingUser.Address = model.Address;

                    // Preserve non-editable fields like Email and Password
                    // db.Entry(existingUser).State = EntityState.Modified;  // No need for this, you are directly updating fields

                    db.SaveChanges();
                    return RedirectToAction("Profile");
                }
            }

            return View(model);
        }


        public ActionResult ChangePassword(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user); // Pass the user object to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(int? id, string currentPassword, string newPassword, string confirmPassword)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            // Check if current password is correct
            if (user.Password != currentPassword)
            {
                ModelState.AddModelError("currentPassword", "كلمة السر غير صحيحة");
                return View(user);
            }

            // Check if the new password and confirm password match
            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "كلمات المرور غير متطابقة");
                return View(user);
            }

            // Update the password if validation passes
            if (ModelState.IsValid)
            {
                user.Password = newPassword; // Update the password
                db.SaveChanges(); // Save changes to the database

                // Set TempData to indicate success
                TempData["PasswordChanged"] = true;
                return RedirectToAction("ChangePassword", new { id = user.UserId }); // Reload the same view
            }

            return View(user); // Return to the view if validation fails
        }




        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
