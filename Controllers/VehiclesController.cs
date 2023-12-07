using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftCarRental.Data;
using SwiftCarRental.Models;

namespace SwiftCarRental.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly SwiftCarRentalDBContext _context;

        public VehiclesController(SwiftCarRentalDBContext context)
        {
            _context = context;
        }

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

        [Authorize]
        public IActionResult Search(DateTime fromDate, DateTime toDate, string type)
        {
            // First, filter by Type
            var filteredByType = _context.Vehicle
                .Where(v => type == null || v.Type == type)
                .ToList();

            // List to hold vehicles that are available in the given date range
            var availableVehicles = new List<Vehicle>();

            // Check each vehicle's availability
            foreach (var vehicle in filteredByType)
            {
                bool isAvailable = true;

                for (int i = 0; i < vehicle.BookingFromDates.Count; i++)
                {
                    DateTime bookedFromDate = vehicle.BookingFromDates[i];
                    DateTime bookedToDate = vehicle.BookingToDates[i];

                    // Check if there's an overlap with the user's desired booking period
                    if (bookedFromDate <= toDate && bookedToDate >= fromDate)
                    {
                        isAvailable = false;
                        break; // Break the loop if any booking overlaps
                    }
                }

                if (isAvailable)
                {
                    availableVehicles.Add(vehicle);
                }
            }
            var vehicleSearchViewModel = new VehicleSearchViewModel
            {
                AvailableVehicles = availableVehicles,
                numberOfHours = CalculateRoundedHours(fromDate,toDate)
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
