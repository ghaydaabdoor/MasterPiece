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
         
        //FARMS
        public ActionResult ShowFarms()
        {
            var farms = db.Farms.ToList(); 
            var farmDetails = db.FarmDetails.ToList(); // Fetch from Farms table

            var viewModel = farms.Select(v => new FarmViewModel
            {
                farm = v,
                farmDetail = farmDetails.FirstOrDefault(d => d.FarmId == v.FarmId) 
            });

            return View(viewModel);
        }

        public ActionResult CreateFarm()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFarm(FarmViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Farms.Add(viewModel.farm);
                db.SaveChanges();

                viewModel.farmDetail.FarmId = viewModel.farm.FarmId;
                viewModel.farmDetail.Price = (int)viewModel.farm.Price;
                viewModel.farmDetail.Name = viewModel.farm.Name;

                // Save the FarmsDetails entity
                db.FarmDetails.Add(viewModel.farmDetail);
                db.SaveChanges();

                // Redirect to the index or show farms page
                return RedirectToAction("ShowFarms");
            }

            return View(viewModel);
        }

        // GET: Farms/Delete/5
        public ActionResult DeleteFarm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Farm farm = db.Farms.Find(id);
            if (farm == null)
            {
                return HttpNotFound();
            }

            return View(farm);
        }

        // POST: Farms/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFarm(int id)
        {
            var farmDet = db.FarmDetails.FirstOrDefault(x => x.FarmId == id);

            if (farmDet != null)
            {
                // Delete the details first if found
                db.FarmDetails.Remove(farmDet);
                db.SaveChanges();
            }

            // Find and delete the farm itself
            var farmOrg = db.Farms.Find(id);
            if (farmOrg != null)
            {
                db.Farms.Remove(farmOrg);
                db.SaveChanges();
            }

            return RedirectToAction("ShowFarms");
        }

        // GET: Farms/Edit/5
        public ActionResult EditFarm(int id)
        {
            var farmID = db.Farms.Find(id); 
            var farmDETAILS= db.FarmDetails.FirstOrDefault(d => d.FarmId == id); // Fetch from FarmDetails table

            if (farmID == null || farmDETAILS == null)
            {
                return HttpNotFound();
            }

            // Populate the ViewModel with data from both tables
            var viewModel = new FarmViewModel
            {
                farm = farmID,
                farmDetail = farmDETAILS
            };

            return View(viewModel);
        }

        // POST: Farms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFarm(FarmViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Update the Farm entity
                var farm11 = db.Farms.Find(viewModel.farm.FarmId);
                if (farm11 != null)
                {
                    farm11.Name = viewModel.farm.Name;
                    farm11.Price = viewModel.farm.Price;
                    farm11.Location = viewModel.farm.Location;
                    farm11.Rate = viewModel.farm.Rate;
                    farm11.ImageUrl = viewModel.farm.ImageUrl;
                    db.Entry(farm11).State = EntityState.Modified;
                }

                // Update the FarmDetails entity
                var farmDetails11 = db.FarmDetails.FirstOrDefault(d => d.FarmId == viewModel.farm.FarmId);
                if (farmDetails11 != null)
                {
                    farmDetails11.ImageUrl1 = viewModel.farmDetail.ImageUrl1;
                    farmDetails11.ImageUrl2 = viewModel.farmDetail.ImageUrl2;
                    farmDetails11.ImageUrl3 = viewModel.farmDetail.ImageUrl3;
                    farmDetails11.ImageUrl4 = viewModel.farmDetail.ImageUrl4;
                    farmDetails11.ImageUrl5 = viewModel.farmDetail.ImageUrl5;
                    farmDetails11.ImageUrl6 = viewModel.farmDetail.ImageUrl6;
                    farmDetails11.Description = viewModel.farmDetail.Description;
                    farmDetails11.Capacity = viewModel.farmDetail.Capacity;
                    farmDetails11.Price = (int)viewModel.farm.Price;
                    farmDetails11.PriceDetails = viewModel.farmDetail.PriceDetails;
                    farmDetails11.Name = viewModel.farm.Name;
                    farmDetails11.Location = viewModel.farmDetail.Location;

                    db.Entry(farmDetails11).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("ShowFarms");
            }

            return View(viewModel);
        }

        // Clothes
        public ActionResult ShowCloths()
        {
            var cloths11 = db.Clothes.ToList();
            var clothDetails11 = db.ClothDetails.ToList(); 

            var viewModel = cloths11.Select(v => new ClothViewModel
            {
                cloth = v,
                clothDetail = clothDetails11.FirstOrDefault(d => d.ClothId == v.ClothId)
            });

            return View(viewModel);
        }

        public ActionResult CreateCloth()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCloth(ClothViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.cloth.Description = viewModel.clothDetail.Description;
                db.Clothes.Add(viewModel.cloth);
                db.SaveChanges();

                viewModel.clothDetail.ClothId = viewModel.cloth.ClothId;
                viewModel.clothDetail.Name = viewModel.cloth.Name;

                // Save the ClothesDetails entity
                db.ClothDetails.Add(viewModel.clothDetail);
                db.SaveChanges();

                // Redirect to the index or show clothes page
                return RedirectToAction("ShowCloths");
            }

            return View(viewModel);
        }

        // GET: Cloth/Delete/5
        public ActionResult DeleteCloth(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cloth cloth = db.Clothes.Find(id);
            if (cloth == null)
            {
                return HttpNotFound();
            }

            return View(cloth);
        }

        // POST: Cloth/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCloth(int id)
        {
            var clothDet = db.ClothDetails.FirstOrDefault(x => x.ClothId == id);

            if (clothDet != null)
            {
                // Delete the details first if found
                db.ClothDetails.Remove(clothDet);
                db.SaveChanges();
            }

            // Find and delete the farm itself
            var clothOrg = db.Clothes.Find(id);
            if (clothOrg != null)
            {
                db.Clothes.Remove(clothOrg);
                db.SaveChanges();
            }

            return RedirectToAction("ShowCloths");
        }

        // GET: Cloth/Edit/5
        public ActionResult EditCloth(int id)
        {
            var clothID = db.Clothes.Find(id);
            var clothDETAILS = db.ClothDetails.FirstOrDefault(d => d.ClothId == id); // Fetch from FarmDetails table

            if (clothID == null || clothDETAILS == null)
            {
                return HttpNotFound();
            }

            // Populate the ViewModel with data from both tables
            var viewModel = new ClothViewModel
            {
                cloth = clothID,
                clothDetail = clothDETAILS,
            };

            return View(viewModel);
        }

        // POST: Cloth/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCloth(ClothViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Update the Farm entity
                var cloth11 = db.Clothes.Find(viewModel.cloth.ClothId);
                if (cloth11 != null)
                {
                    cloth11.Name = viewModel.cloth.Name;
                    cloth11.Color = viewModel.cloth.Color;
                    cloth11.Description = viewModel.cloth.Description;
                    cloth11.Rate = viewModel.cloth.Rate;
                    cloth11.ImageUrl = viewModel.cloth.ImageUrl;
                    cloth11.Category = viewModel.cloth.Category;
                    cloth11.Description=viewModel.clothDetail.Description;
                    db.Entry(cloth11).State = EntityState.Modified;
                }

                // Update the FarmDetails entity
                var clothDetails11 = db.ClothDetails.FirstOrDefault(d => d.ClothId == viewModel.cloth.ClothId);
                if (clothDetails11 != null)
                {
                    clothDetails11.ImageUrl1 = viewModel.clothDetail.ImageUrl1;
                    clothDetails11.ImageUrl2 = viewModel.clothDetail.ImageUrl2;
                    clothDetails11.ImageUrl3 = viewModel.clothDetail.ImageUrl3;
                    clothDetails11.ImageUrl4 = viewModel.clothDetail.ImageUrl4;
                    clothDetails11.Color = viewModel.cloth.Color;
                    clothDetails11.Description = viewModel.clothDetail.Description;
                    clothDetails11.ClothMaterial = viewModel.clothDetail.ClothMaterial;
                    clothDetails11.Features = viewModel.clothDetail.Features;
                    clothDetails11.PriceDetails = viewModel.clothDetail.PriceDetails;
                    clothDetails11.Name = viewModel.cloth.Name;
                   

                    db.Entry(clothDetails11).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("ShowCloths");
            }

            return View(viewModel);
        }

        // Users
        public ActionResult ShowUsers()
        {
            return View(db.Users.ToList());
        }

        // GET: Food/Delete/5
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);  // Find the food item by ID
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Food/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("ShowUsers");
        }

        // Food
        public ActionResult ShowFood()
        {
            return View(db.Foods.ToList());
        }

        public ActionResult CreateFood()
        {
            return View();
        }

        // POST: Food/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFood([Bind(Include = "FoodId,Name,Price,Description,Rate,ImageUrl")] Food food)  // Bind the properties of the Food model
        {
            if (ModelState.IsValid)
            {
                db.Foods.Add(food);  
                db.SaveChanges();  
                return RedirectToAction("ShowFood");
            }

            return View(food);
        }
        public ActionResult EditFood(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);  // Find the food item by ID
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Food/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFood([Bind(Include = "FoodId,Name,Price,Description,Rate,ImageUrl")] Food food)  // Bind the properties of the Food model
        {
            if (ModelState.IsValid)
            {
                db.Entry(food).State = EntityState.Modified;  // Modify the existing food item
                db.SaveChanges();
                return RedirectToAction("ShowFood");
            }
            return View(food);
        }

        // GET: Food/Delete/5
        public ActionResult DeleteFood(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);  // Find the food item by ID
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Food/Delete/5
        [HttpPost, ActionName("DeleteFood")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = db.Foods.Find(id);  // Find the food item by ID
            db.Foods.Remove(food);  // Remove the food item from the database
            db.SaveChanges();
            return RedirectToAction("ShowFood");
        }


        // Clothes
        public ActionResult ShowPhotgraphers()
        {
            var photog11 = db.Photographers.Include(p=>p.PhotographerDetails).ToList();
            var photogDetails11 = db.PhotographerDetails.ToList();

            var viewModel = photog11.Select(v => new PhotographViewModel
            {
                photographer = v,
                photographerDetail = photogDetails11.FirstOrDefault(d => d.PhotographerId == v.PhotographerId)
            });

            return View(viewModel);
        }

        public ActionResult CreatePhotgraphers()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhotgraphers(PhotographViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var x = viewModel.photographer.Price.ToString();
                 x = viewModel.photographerDetail.PriceDetails;
                db.Photographers.Add(viewModel.photographer);
                db.SaveChanges();

                viewModel.photographerDetail.PhotographerId = viewModel.photographer.PhotographerId;

                // Save the ClothesDetails entity
                db.PhotographerDetails.Add(viewModel.photographerDetail);
                db.SaveChanges();

                // Redirect to the index or show clothes page
                return RedirectToAction("ShowPhotgraphers");
            }

            return View(viewModel);
        }

        // GET: Cloth/Delete/5
        public ActionResult DeletePhotgraphers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Photographer photog = db.Photographers.Find(id);
            if (photog == null)
            {
                return HttpNotFound();
            }

            return View(photog);
        }

        // POST: Cloth/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePhotgraphers(int id)
        {
            var photogDet = db.PhotographerDetails.FirstOrDefault(x => x.PhotographerId == id);

            if (photogDet != null)
            {
                // Delete the details first if found
                db.PhotographerDetails.Remove(photogDet);
                db.SaveChanges();
            }

            // Find and delete the farm itself
            var photogOrg = db.Photographers.Find(id);
            if (photogOrg != null)
            {
                db.Photographers.Remove(photogOrg);
                db.SaveChanges();
            }

            return RedirectToAction("ShowPhotgraphers");
        }

        // GET: Cloth/Edit/5
        public ActionResult EditPhotgraphers(int id)
        {
            var photogID = db.Photographers.Find(id);
            var photogDETAILS = db.PhotographerDetails.FirstOrDefault(d => d.PhotographerId == id); // Fetch from FarmDetails table

            if (photogID == null || photogDETAILS == null)
            {
                return HttpNotFound();
            }

            // Populate the ViewModel with data from both tables
            var viewModel = new PhotographViewModel
            {
                photographer = photogID,
                photographerDetail = photogDETAILS,
            };

            return View(viewModel);
        }

        // POST: Cloth/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhotgraphers(PhotographViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Update the Farm entity
                var photog11 = db.Photographers.Find(viewModel.photographer.PhotographerId);
                if (photog11 != null)
                {
                    photog11.Name = viewModel.photographer.Name;
                    var x = photog11.Price.ToString();
                    x = viewModel.photographerDetail.PriceDetails;
                    photog11.Rate = viewModel.photographer.Rate;
                    photog11.ImageUrl = viewModel.photographer.ImageUrl;
                    db.Entry(photog11).State = EntityState.Modified;
                }

                // Update the FarmDetails entity
                var photogDetails11 = db.PhotographerDetails.FirstOrDefault(d => d.PhotographerId == viewModel.photographer.PhotographerId);
                if (photogDetails11 != null)
                {
                    photogDetails11.ImageUrl1 = viewModel.photographerDetail.ImageUrl1;
                    photogDetails11.ImageUrl2 = viewModel.photographerDetail.ImageUrl2;
                    photogDetails11.ImageUrl3 = viewModel.photographerDetail.ImageUrl3;
                    photogDetails11.ImageUrl4 = viewModel.photographerDetail.ImageUrl4;
                    photogDetails11.ImageUrl5 = viewModel.photographerDetail.ImageUrl5;
                    photogDetails11.ImageUrl6 = viewModel.photographerDetail.ImageUrl6;
                    photogDetails11.PriceDetails = viewModel.photographerDetail.PriceDetails;
                    photogDetails11.Description = viewModel.photographerDetail.Description;

                    db.Entry(photogDetails11).State = EntityState.Modified;
                }

                db.SaveChanges();
                return RedirectToAction("ShowPhotgraphers");
            }

            return View(viewModel);
        }
    }

}
