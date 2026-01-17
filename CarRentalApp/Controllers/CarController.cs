using CarRentalApp.Areas.Identity.Data;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Controllers
{
    public class CarController : Controller
    {
        private readonly AppDbContext _context;
        public CarController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CarController
        public ActionResult Index()
        {
            var cars = _context.Car.Include(c => c.CarCategory).ToList();
            return View(cars);
        }

        // GET: CarController/Details/5
        public ActionResult Details(int id)
        {
            var car = _context.Car.Include(c => c.CarCategory).FirstOrDefault(c => c.Id == id);
            if (car == null)
                return NotFound();
            return View(car);
        }

        // GET: CarController/Create
        public ActionResult Create()
        {
            ViewBag.CarCategories = new SelectList(_context.CarCategories, "Id", "Name");
            return View();
        }

        // POST: CarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Car car)
        {
            try
            {
                _context.Car.Add(car);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var car = _context.Car.Find(id);

            if (car == null)
                return NotFound();

            ViewBag.CarCategories = new SelectList(
                _context.CarCategories,
                "Id",
                "Name",
                car.CarCategoryId 
            );

            return View(car);
        }

        // POST: CarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Car car)
        {
            try
            {
                _context.Car.Update(car);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var car = _context.Car.Include(c => c.CarCategory).FirstOrDefault(c => c.Id == id);
            if (car == null)
                return NotFound();
            return View(car);
        }

        // POST: CarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, Car car)
        {
            try
            {
                _context.Car.Remove(car);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
