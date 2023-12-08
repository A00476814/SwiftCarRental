using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftCarRental.Areas.Identity.Data;
using SwiftCarRental.Data;
using SwiftCarRental.Models;

namespace SwiftCarRental.Controllers
{
    public class BookingsController : Controller
    {
        private readonly SwiftCarRentalDBContext _context;
        private readonly userDBContext _userDbContext;

        public BookingsController(SwiftCarRentalDBContext context, userDBContext userDbContext)
        {
            _context = context;
            _userDbContext = userDbContext;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var swiftCarRentalDBContext = _context.Booking.Include(b => b.Vehicle);
            return View(await swiftCarRentalDBContext.ToListAsync());
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create(int VehicleId, DateTime FromDate, DateTime ToDate)
        {
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id");
            var userEmail = User.Identity.Name;
            var bookingDetails = new BookingDetails
            {
                Vehicle = _context.Vehicle.Find(VehicleId),
                User = _userDbContext.Users.FirstOrDefault(x => x.Email == userEmail),
                FromDate = FromDate,
                ToDate = ToDate
            };
            return View(bookingDetails);
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,VehicleId,LicenceNo,StartDate,EndDate")] Booking booking)
        {
            booking.Vehicle = _context.Vehicle.FirstOrDefault(v => v.Id == booking.VehicleId);
            var vehicle = _context.Vehicle.Include(v => v.Bookings).FirstOrDefault(v => v.Id == booking.VehicleId);
            booking.Vehicle = vehicle;
            ModelState.Remove("Vehicle");
            var userEmail = User.Identity.Name;
            var currentUser = _userDbContext.Users.FirstOrDefault(x => x.Email == userEmail);
            PaymentHelperModel paymentHelperModel = new PaymentHelperModel();
            paymentHelperModel.Country = currentUser.Nation;
            paymentHelperModel.PaymentDetails = new PaymentDetails();
            if (vehicle != null)
            {
                vehicle.Bookings.Add(booking);
                if (ModelState.IsValid)
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                    return View("~/Views/PaymentDetails/create.cshtml", paymentHelperModel);
                }
                else
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .Select(x => new { x.Key, x.Value.Errors })
                        .ToArray();

                    // Now 'errors' contains the properties that failed to validate and their corresponding error messages.
                }
            }

            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", booking.VehicleId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", booking.VehicleId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,VehicleId,LicenceNo,StartDate,EndDate")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicle, "Id", "Id", booking.VehicleId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Vehicle)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            if (booking != null)
            {
                _context.Booking.Remove(booking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
