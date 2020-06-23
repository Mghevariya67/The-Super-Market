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
    public class CartsController : Controller
    {
        private readonly TheIndianMarketContext _context;

        public CartsController(TheIndianMarketContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var Context = _context.Cart.Include(c => c.Customer).Include(c => c.Product).Where(p => p.CustomerId == Convert.ToInt32(HttpContext.Session.GetString("CustomerID")));
            HttpContext.Session.SetString("TotalItem", Context.Count().ToString());
            ViewData["Sum"] = Context.Sum(p => p.Product.PricePerCostUnit);
            return View(await Context.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        public async Task<IActionResult> Create([Bind ("productId, CustomerID")] Cart _cart)
        {
            if (Request.Query["PID"].Any() && Request.Query["CustID"].Any())
            {
                _cart.ProductId = Convert.ToInt32(Request.Query["PID"]);
                _cart.CustomerId = Convert.ToInt32(Request.Query["CustID"]);
            }
            
           
            if (ModelState.IsValid)
            {
               

                _context.Add(_cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           /*ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Password", cart.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "CostUnit", cart.ProductId);*/
            return View(_cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Password", cart.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "CostUnit", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,ProductId")] Cart cart)
        {
            
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Password", cart.CustomerId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "CostUnit", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
     /*  public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }
        */
        // POST: Carts/Delete/5
       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            var _cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(_cart);
            await _context.SaveChangesAsync();

            var cartContext = _context.Cart.Where(p => p.CustomerId == Convert.ToInt32(HttpContext.Session.GetString("CustomerID")));
            HttpContext.Session.SetString("TotalItem", cartContext.Count().ToString());
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
    }
}
