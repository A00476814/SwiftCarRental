using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftCarRental.Data;
using SwiftCarRental.Models;

namespace SwiftCarRental.Controllers
{
    public class PaymentDetailsController : Controller
    {
        private readonly SwiftCarRentalDBContext _context;

        public PaymentDetailsController(SwiftCarRentalDBContext context)
        {
            _context = context;
        }

        // GET: PaymentDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.PaymentDetails.ToListAsync());
        }

        // GET: PaymentDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentDetails = await _context.PaymentDetails
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (paymentDetails == null)
            {
                return NotFound();
            }

            return View(paymentDetails);
        }

        // GET: PaymentDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,NameOnCard,CreditCardType,CreditCardNumber,ExpirationDate,CVV,PostalCode")] PaymentDetails paymentDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paymentDetails);
                await _context.SaveChangesAsync();
                return View("PaymentConfirmation");
            }
            else
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();

                // Now 'errors' contains the properties that failed to validate and their corresponding error messages.
            }
            return View(paymentDetails);
        }

        // GET: PaymentDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentDetails = await _context.PaymentDetails.FindAsync(id);
            if (paymentDetails == null)
            {
                return NotFound();
            }
            return View(paymentDetails);
        }

        // POST: PaymentDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CreditCardType,CreditCardNumber,ExpirationDate,CVV,PostalCode")] PaymentDetails paymentDetails)
        {
            if (id != paymentDetails.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentDetailsExists(paymentDetails.PaymentId))
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
            return View(paymentDetails);
        }

        // GET: PaymentDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentDetails = await _context.PaymentDetails
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (paymentDetails == null)
            {
                return NotFound();
            }

            return View(paymentDetails);
        }

        [HttpPost]
        public IActionResult ValidatePayment(PaymentDetails paymentDetails)
        {
            if (ModelState.IsValid)
            {
                return Json(new { hasErrors = false });
            }
            else
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();
                if (errors.Any())
                {
                    var firstError = errors.First().Errors.First().ErrorMessage;
                    return Json(new { hasErrors = true, errorMessage = firstError });
                }
                // Now 'errors' contains the properties that failed to validate and their corresponding error messages.
            }
            return Json(new { hasErrors = false });
        }

        // POST: PaymentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paymentDetails = await _context.PaymentDetails.FindAsync(id);
            if (paymentDetails != null)
            {
                _context.PaymentDetails.Remove(paymentDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentDetailsExists(int id)
        {
            return _context.PaymentDetails.Any(e => e.PaymentId == id);
        }
    }
}
