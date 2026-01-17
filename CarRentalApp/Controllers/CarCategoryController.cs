using CarRentalApp.Areas.Identity.Data;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Controllers
{
    public class CarCategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CarCategoryController(AppDbContext context)
        {
            _context = context;
        }
        // GET: CarCategoryController
        public ActionResult Index()
        {
            return View(_context.CarCategories.ToList());
        }

        // GET: CarCategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View(_context.CarCategories.Find(id));
        }

        // GET: CarCategoryController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarCategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CarCategory carCat)
        {
            try
            {
                _context.CarCategories.Add(carCat);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarCategoryController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(_context.CarCategories.Find(id));
        }

        // POST: CarCategoryController/Edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CarCategory carCat)
        {
            try
            {
                _context.CarCategories.Update(carCat);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CarCategoryController/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View(_context.CarCategories.Find(id));
        }

        // POST: CarCategoryController/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CarCategory carCat)
        {
            try
            {
                _context.CarCategories.Remove(carCat);
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
