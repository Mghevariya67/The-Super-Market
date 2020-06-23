using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheIndianSuperMarket.Models;

namespace TheIndianSuperMarket.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly TheIndianMarketContext _context;

        public PaymentsController(TheIndianMarketContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var theIndianMarketContext = _context.Payment.Include(p => p.Cart).Include(p => p.Customer);
            return View(await theIndianMarketContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Cart)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Password");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,CartId,CustomerId,NameOnCard,CardNumber,Expiration,Cvv")] Payment payment)
        {
            var cartContext = _context.Cart.Include(p => p.Customer).Where(p => p.CustomerId == Convert.ToInt32(HttpContext.Session.GetString("CustomerID"))).FirstOrDefault(); ;
            payment.CustomerId =Convert.ToInt32(HttpContext.Session.GetString("CustomerID"));
            payment.CartId = cartContext.Id ;
            var paymentContext = _context.Payment;
           
            if (paymentContext == null)
            {
                payment.PaymentId = 1;
            }
            else
            {
                payment.PaymentId = paymentContext.Max(p => p.PaymentId) +1;
            }
           
            if (ModelState.IsValid)
            {
                _context.Add(payment);
               
                HttpContext.Session.SetString("TotalItem","0");

                var _cart = _context.Cart.Where(p=>p.CustomerId == Convert.ToInt32(HttpContext.Session.GetString("CustomerID"))).FirstOrDefault();
              
               // _context.Cart.Remove(_cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", payment.CartId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Password", payment.CustomerId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", payment.CartId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Password", payment.CustomerId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CartId,CustomerId,NameOnCard,CardNumber,Expiration,Cvv")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["CartId"] = new SelectList(_context.Cart, "Id", "Id", payment.CartId);
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Password", payment.CustomerId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Cart)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.PaymentId == id);
        }
    }
}
