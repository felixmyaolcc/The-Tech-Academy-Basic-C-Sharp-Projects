using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // This is where you add the CalculateQuote method
        private decimal CalculateQuote(Insuree insuree)
        {
            decimal basePrice = 50;
            decimal totalPrice = basePrice;

            // Add $100 if the user is 18 or under
            if (insuree.DateOfBirth.AddYears(18) > DateTime.Now)
            {
                totalPrice += 100;
            }
            // Add $50 if the user is between 19 and 25
            else if (insuree.DateOfBirth.AddYears(25) > DateTime.Now)
            {
                totalPrice += 50;
            }
            // Add $25 if the user is 26 or older
            else
            {
                totalPrice += 25;
            }

            // Add $25 if the car's year is before 2000
            if (insuree.CarYear < 2000)
            {
                totalPrice += 25;
            }
            // Add $25 if the car's year is after 2015
            else if (insuree.CarYear > 2015)
            {
                totalPrice += 25;
            }

            // Add $25 if the car's Make is a Porsche
            if (insuree.CarMake.ToLower() == "porsche")
            {
                totalPrice += 25;
                // Add an additional $25 if the model is a 911 Carrera
                if (insuree.CarModel.ToLower() == "911 carrera")
                {
                    totalPrice += 25;
                }
            }

            // Add $10 for every speeding ticket
            totalPrice += insuree.SpeedingTickets * 10;

            // Add 25% if the user has had a DUI
            if (insuree.DUI)
            {
                totalPrice *= 1.25m;
            }

            // Add 50% if the user selects full coverage
            if (insuree.CoverageType == "Full")
            {
                totalPrice *= 1.50m;
            }

            return totalPrice;
        }



        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Calculate the quote before saving to the database
                insuree.Quote = CalculateQuote(insuree);

                db.Insurees.Add(insuree);  // Add the insuree to the database
                db.SaveChanges();  // Save changes to the database

                return RedirectToAction("Index");  // Redirect to the Index page
            }

            return View(insuree);  // Return the Create view if the model state is not valid
        }


        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                // Recalculate the quote when editing the insuree
                insuree.Quote = CalculateQuote(insuree);

                // Update the insuree's record in the database
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(insuree);
        }


        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Insuree/Admin
        public ActionResult Admin()
        {
            // Get all insurees with the relevant properties (FirstName, LastName, EmailAddress, Quote)
            var insurees = db.Insurees.Select(i => new
            {
                i.FirstName,
                i.LastName,
                i.EmailAddress,
                i.Quote
            }).ToList();

            return View(insurees);
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
