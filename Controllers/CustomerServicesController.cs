using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SwiftCarRental.Data;
using SwiftCarRental.Models;

namespace SwiftCarRental.Controllers
{
    public class CustomerServicesController : Controller
    {
        private readonly SwiftCarRentalDBContext _context;

        public CustomerServicesController(SwiftCarRentalDBContext context)
        {
            _context = context;
        }

        // GET: CustomerServices
        public async Task<IActionResult> Index()
        {
            return View(await _context.CustomerService.ToListAsync());
        }

        // GET: CustomerServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerService = await _context.CustomerService
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customerService == null)
            {
                return NotFound();
            }

            return View(customerService);
        }

        // GET: CustomerServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Question,Answer")] CustomerService customerService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customerService);
        }

        // GET: CustomerServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerService = await _context.CustomerService.FindAsync(id);
            if (customerService == null)
            {
                return NotFound();
            }
            return View(customerService);
        }

        // POST: CustomerServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Question,Answer")] CustomerService customerService)
        {
            if (id != customerService.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerServiceExists(customerService.ID))
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
            return View(customerService);
        }

        // GET: CustomerServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerService = await _context.CustomerService
                .FirstOrDefaultAsync(m => m.ID == id);
            if (customerService == null)
            {
                return NotFound();
            }

            return View(customerService);
        }

        // POST: CustomerServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerService = await _context.CustomerService.FindAsync(id);
            if (customerService != null)
            {
                _context.CustomerService.Remove(customerService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Submit()
        {
            var faqs = _context.CustomerService.ToList();
            return View(faqs);
        }

        [HttpPost]
        public ActionResult Submit(string email, string question)
        {

            SmtpClient smtpclient = new SmtpClient();
            smtpclient.Host = "smtp.gmail.com";
            smtpclient.Port = 587;

            string senderGmail = "swiftcarrentalservices@gmail.com";
            string receiverGmail = "khevaria.rishabh@outlook.com";
            MailAddress sendAddress = new MailAddress(senderGmail);
            MailAddress receiveAddress = new MailAddress(receiverGmail);


            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = "Swift Car Rental Customer Service"; 
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;


            mailMessage.Body = "<h2>Swift Car Rental Customer Service</h2>" +
                               "<p><strong>User Email:</strong> " + email + "</p>" +
                               "<p><strong>Question:</strong><br/>" + question + "</p>";


            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            mailMessage.From = sendAddress;
            mailMessage.To.Add(receiveAddress);

 
            smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;


            smtpclient.EnableSsl = true;


            smtpclient.UseDefaultCredentials = false;

  
            NetworkCredential networkCredential = new NetworkCredential(senderGmail, "ydzm dyxx kwro ysho");
            smtpclient.Credentials = networkCredential;


            smtpclient.Send(mailMessage);

            var faqs = _context.CustomerService.ToList();
            return View(faqs);
        }
        private bool CustomerServiceExists(int id)
        {
            return _context.CustomerService.Any(e => e.ID == id);
        }
    }
}
