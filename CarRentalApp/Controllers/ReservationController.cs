using CarRentalApp.Areas.Identity.Data;
using CarRentalApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarRentalApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public ReservationController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ReservationController
        public ActionResult Index()
        {
            var reservations = _context.Reservations
            .Include(r => r.Car)  
            .Include(r => r.User) 
            .ToList();

            return View(reservations);
        }

        // GET: ReservationController/Details/5
        public ActionResult Details(int id)
        {
            var reservation = _context.Reservations
            .Include(r => r.Car)  
            .Include(r => r.User) 
            .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: ReservationController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Cars = new SelectList(_context.Car.ToList(), "Id", "FullName");
            return View();
        }

        // POST: ReservationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            reservation.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ModelState.Remove("UserId"); 

            if (ModelState.IsValid)
            {
                bool isCarOccupied = _context.Reservations.Any(r =>
                    r.CarId == reservation.CarId &&
                    reservation.StartDate < r.EndDate &&
                    reservation.EndDate > r.StartDate);

                if (isCarOccupied)
                {
                    ModelState.AddModelError("", "Przepraszamy, ten samochód jest już zarezerwowany w wybranym terminie.");

                    ViewData["CarId"] = new SelectList(_context.Car, "Id", "Model", reservation.CarId);
                    return View(reservation);
                }

                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CarId"] = new SelectList(_context.Car , "Id", "Model", reservation.CarId);
            return View(reservation);
        }
        [Authorize]
        public async Task<IActionResult> MyReservations()
        {
            // Pobieramy ID zalogowanego użytkownika
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myReservations = await _context.Reservations
                .Include(r => r.Car)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();

            return View(myReservations);
        }
        // GET: ReservationController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var reservation = _context.Reservations.Find(id);

            if (reservation == null)
                return NotFound();

            ViewBag.Cars = new SelectList(
                _context.Car.ToList(),
                "Id",
                "FullName",
                reservation.CarId
            );

            return View(reservation);
        }

        // POST: ReservationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> EditAsync(int id, Reservation reservation)
        {
            if (id != reservation.Id) return NotFound();

            var existingReservation = await _context.Reservations.AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation.UserId == null && existingReservation != null)
            {
                reservation.UserId = existingReservation.UserId;
            }

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: ReservationController/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Car)  
                .Include(r => r.User) 
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: ReservationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id, Reservation reservation)
        {
            try
            {
                _context.Reservations.Remove(reservation);
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
