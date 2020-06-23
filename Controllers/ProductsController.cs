using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheIndianSuperMarket.Models;

namespace TheIndianSuperMarket.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TheIndianMarketContext _context;

        public ProductsController(TheIndianMarketContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string depart_Name)
        {
            
            if (Request.Query["DepartmentName"].Any())
            {
                depart_Name = Request.Query["DepartmentName"].ToString();
            }
            else
            {
                depart_Name = "Snacks";
            }
            var theIndianMarketContext = _context.Products.Include(p => p.DepartmentNameNavigation).Where(p => p.DepartmentName == depart_Name);
            return View(await theIndianMarketContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.DepartmentNameNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "DepartmentName", "DepartmentName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,PricePerCostUnit,CostUnit,DepartmentName,QuantityInStock,Brand,ProductionDate,BestBeforeDate,Plu,Upc,Organic,Cut,Animal")] Products products)
        {
            var prodContext = _context.Products;
            int id = prodContext.Max(p => p.ProductId);
            products.ProductId = id + 1;
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                TempData["Message"] = products.ProductName + " has been created succesfully!";
                return RedirectToAction("Products","Admins");
            }
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "DepartmentName", "DepartmentName", products.DepartmentName);
            return View(products);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "DepartmentName", "DepartmentName", products.DepartmentName);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,PricePerCostUnit,CostUnit,DepartmentName,QuantityInStock,Brand,ProductionDate,BestBeforeDate,Plu,Upc,Organic,Cut,Animal")] Products products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["Message"] = products.ProductName + " has been edited succesfully!";
                return RedirectToAction("Products", "Admins");
            }
            ViewData["DepartmentName"] = new SelectList(_context.Departments, "DepartmentName", "DepartmentName", products.DepartmentName);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.DepartmentNameNavigation)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
