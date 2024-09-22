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
    public class UsersController : Controller
    {
        private MasterPieceEntities1 db = new MasterPieceEntities1();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //// GET: Users/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

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
                ModelState.AddModelError("Email", " البريد الإلكتروني موجود بالفعل. يرجى استخدام بريد إلكتروني آخر");
            }
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            // Fetch the user by email
            var user = db.Users.FirstOrDefault(p => p.Email == model.Email);

            // Check if user exists
            if (user == null)
            {
                ModelState.AddModelError("Email", "البريد الالكتروني غير صحيح");
            }
            else
            {
                if (model.Password != user.Password) 
                {
                    ModelState.AddModelError("Password", "كلمة السر غير صحيحة");
                }
            }

            if (ModelState.IsValid)
            {
                Session.Clear();
                Session["UserId"] = user.UserId;
                return RedirectToAction("Index", "Home");
            }

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

            return View(user);
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
