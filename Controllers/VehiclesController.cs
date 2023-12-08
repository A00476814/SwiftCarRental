using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftCarRental.Areas.Identity.Data;
using SwiftCarRental.Data;
using SwiftCarRental.Models;

namespace SwiftCarRental.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly SwiftCarRentalDBContext _context;
        private readonly userDBContext _userDBContext;

        public VehiclesController(SwiftCarRentalDBContext context,userDBContext userDBContext)
        {
            _context = context;

        }
        //Identity/Account/Manage
        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicle.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LicensePlate,Model,Brand,ModelYear,Type,Odometer,NoOfSeats,PricePerHour,BookingFromDates,BookingToDates")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LicensePlate,Model,Brand,ModelYear,Type,Odometer,NoOfSeats,PricePerHour,BookingFromDates,BookingToDates")] Vehicle vehicle)
        {
            if (id != vehicle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.Id))
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
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       // [Authorize]
        public IActionResult Search(DateTime fromDate, DateTime toDate, string type)
        {

            var availableVehicles = _context.Vehicle
                                    .Include(v => v.Bookings)
                                    .Where(v => type == null || v.Type == type)
                                    .Where(v => !v.Bookings.Any(b => b.StartDate <= toDate && b.EndDate >= fromDate))
                                    .ToList();

            var vehicleSearchViewModel = new VehicleSearchViewModel
            {
                AvailableVehicles = availableVehicles,
                numberOfHours = CalculateRoundedHours(fromDate,toDate),
                fromDate = fromDate,
                toDate = toDate   
            };

            return View(vehicleSearchViewModel);
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicle.Any(e => e.Id == id);
        }

        private int CalculateRoundedHours(DateTime start, DateTime end)
        {
            if (end < start)
            {
                throw new ArgumentException("End date must be greater than or equal to start date.");
            }

            TimeSpan duration = end - start;
            double totalHours = duration.TotalHours;

            return (int)Math.Ceiling(totalHours);
        }
    }
}
