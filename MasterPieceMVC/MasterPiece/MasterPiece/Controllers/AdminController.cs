using MasterPiece.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MasterPiece.Controllers
{
    public class AdminController : Controller
    {
        private MasterPieceEntities1 db = new MasterPieceEntities1();

        public ActionResult ShowVenues()
        {
            var venues = db.WeddingVenues.ToList(); // Fetch from WeddingVenue table
            var venueDetails = db.WeddingVenueDetails.ToList(); // Fetch from WeddingVenueDetails table

            var viewModel = venues.Select(v => new WeddingVenueViewModel
            {
                Venue = v,
                VenueDetails = venueDetails.FirstOrDefault(d => d.VenueId == v.VenueId) // Match based on VenueId or some key
            });

            return View(viewModel);
        }

        public ActionResult CreateVenue()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateVenue(WeddingVenueViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Save the WeddingVenue entity
                db.WeddingVenues.Add(viewModel.Venue);
                db.SaveChanges();

                // Ensure the VenueId is correctly assigned to the details
                viewModel.VenueDetails.VenueId = viewModel.Venue.VenueId;
                viewModel.VenueDetails.Price = (int)viewModel.Venue.Price;
                viewModel.VenueDetails.Name = viewModel.Venue.Name;
                viewModel.VenueDetails.Capacity = 200;

                // Save the WeddingVenueDetails entity
                db.WeddingVenueDetails.Add(viewModel.VenueDetails);
                db.SaveChanges();

                // Redirect to the index or show venues page
                return RedirectToAction("ShowVenues");
            }

            return View(viewModel);
        }

        // GET: WeddingVenues/Delete/5
        public ActionResult DeleteVenue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the venue by its ID
            WeddingVenue weddingVenue = db.WeddingVenues.Find(id);
            if (weddingVenue == null)
            {
                return HttpNotFound();
            }

            return View(weddingVenue);
        }

        // POST: WeddingVenues/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVenue(int id)
        {
            // Find the WeddingVenueDetails using the venue ID
            var weddingVenueDet = db.WeddingVenueDetails.FirstOrDefault(x => x.VenueId == id);

            if (weddingVenueDet != null)
            {
                // Delete the details first if found
                db.WeddingVenueDetails.Remove(weddingVenueDet);
                db.SaveChanges();
            }

            // Find and delete the venue itself
            var weddingVenueOrg = db.WeddingVenues.Find(id);
            if (weddingVenueOrg != null)
            {
                db.WeddingVenues.Remove(weddingVenueOrg);
                db.SaveChanges();
            }

            return RedirectToAction("ShowVenues");
        }

        // GET: WeddingVenues/Edit/5
        public ActionResult EditVenue(int id)
        {
            var venue = db.WeddingVenues.Find(id); // Fetch from WeddingVenues table
            var venueDetails = db.WeddingVenueDetails.FirstOrDefault(d => d.VenueId == id); // Fetch from WeddingVenueDetails table

            if (venue == null || venueDetails == null)
            {
                return HttpNotFound();
            }

            // Populate the ViewModel with data from both tables
            var viewModel = new WeddingVenueViewModel
            {
                Venue = venue,
                VenueDetails = venueDetails
            };

            return View(viewModel);
        }

        // POST: WeddingVenues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditVenue(WeddingVenueViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Update the WeddingVenue entity
                var venue = db.WeddingVenues.Find(viewModel.Venue.VenueId);
                if (venue != null)
                {
                    venue.Name = viewModel.Venue.Name;
                    venue.Price = viewModel.Venue.Price;
                    venue.Location = viewModel.Venue.Location;
                    venue.Rate = viewModel.Venue.Rate;
                    venue.ImageUrl = viewModel.Venue.ImageUrl;
                    db.Entry(venue).State = EntityState.Modified;
                }

                // Update the WeddingVenueDetails entity
                var venueDetails = db.WeddingVenueDetails.FirstOrDefault(d => d.VenueId == viewModel.Venue.VenueId);
                if (venueDetails != null)
                {
                    venueDetails.ImageUrl1 = viewModel.VenueDetails.ImageUrl1;
                    venueDetails.ImageUrl2 = viewModel.VenueDetails.ImageUrl2;
                    venueDetails.ImageUrl3 = viewModel.VenueDetails.ImageUrl3;
                    venueDetails.ImageUrl4 = viewModel.VenueDetails.ImageUrl4;
                    venueDetails.ImageUrl5 = viewModel.VenueDetails.ImageUrl5;
                    venueDetails.ImageUrl6 = viewModel.VenueDetails.ImageUrl6;
                    venueDetails.Description = viewModel.VenueDetails.Description;
                    venueDetails.Services = viewModel.VenueDetails.Services;
                    venueDetails.CapacityNotes = viewModel.VenueDetails.CapacityNotes;
                    venueDetails.Capacity = 200;
                    venueDetails.Price = (int)viewModel.Venue.Price;
                    venueDetails.Name = viewModel.Venue.Name;
                    venueDetails.Location = viewModel.VenueDetails.Location;

                    db.Entry(venueDetails).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("ShowVenues");
            }

            return View(viewModel);
        }

        //[HttpPost]
        //public ActionResult UpdateImageUrl(int venueId, string newImageUrl)
        //{
        //    // Find the venue in the database
        //    var venue = db.WeddingVenues.Find(venueId);
        //    if (venue != null)
        //    {
        //        // Update the image URL
        //        venue.ImageUrl = newImageUrl;
        //        db.SaveChanges();
        //    }

        //    return Json(new { success = true });
        //}



    }
}