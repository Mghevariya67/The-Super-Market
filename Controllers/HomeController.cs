using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheIndianSuperMarket.Models;

namespace TheIndianSuperMarket.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly TheIndianMarketContext _context;

        public HomeController(TheIndianMarketContext context)
        {
            _context = context;
        }
        public IActionResult HomePage(String Cust_ID)
        {
            if (Cust_ID != null)
            {
                HttpContext.Session.SetString("CustomerID", Cust_ID);
                
            }
            else if (Request.Query["Cust_ID"].Any())
            {
                HttpContext.Session.SetString("CustomerID", Request.Query["Cust_ID"]);
           
            }
            else if (HttpContext.Session.GetString("CustomerID") != null)
            {
                Cust_ID = HttpContext.Session.GetString("Cust_ID");
              
            }
            else
            {
           
                return RedirectToAction("Index");
            }
            var cartContext = _context.Cart.Where(p => p.CustomerId == Convert.ToInt32(HttpContext.Session.GetString("CustomerID")));
            HttpContext.Session.SetString("TotalItem", cartContext.Count().ToString());
            var CustomerContext = _context.Customers.Where(p => p.CustomerId == Convert.ToInt32(HttpContext.Session.GetString("CustomerID"))).FirstOrDefault();
            HttpContext.Session.SetString("CustomerEmail", CustomerContext.CustomerEmail);
            HttpContext.Session.SetString("CustomerName", CustomerContext.CustomerFirstName +" " + CustomerContext.CustomerLastName);
            ViewData["CustomerEmail"] = CustomerContext.CustomerEmail;
          
            return View();
        }
        public IActionResult Index()
        {
            if(HttpContext.Session.GetString("CustomerID") != null)
            {
                RedirectToAction(nameof(HomePage));
            }
                    return View();
            
        }
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("CustomerID") != null)
            {
                RedirectToAction(nameof(HomePage));
            }
            return View();

        }

        [HttpPost]
        public IActionResult Index([Bind("CustomerEmail,Password")] Customers customers)
        {

            
            var CustomerContext = _context.Customers.Where(p => p.CustomerEmail == customers.CustomerEmail && p.Password == customers.Password).FirstOrDefault();
            //Debug.Print("CustomerContext.CustomerId"+CustomerContext.CustomerId.ToString());
            var AdminContext = _context.Admin.Where(p => p.Email == customers.CustomerEmail && p.Password == customers.Password).FirstOrDefault();
            if (CustomerContext != null ) 
            {
                return RedirectToAction(nameof(HomePage), new { Cust_id = CustomerContext.CustomerId });

            }
            else if(AdminContext != null)
            {
                return RedirectToAction(nameof(Index), "Admins");
            }
         
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(IFormCollection form)
        {
            string name = form["name"].ToString();
            string email = form["mail"].ToString();
            string subject = form["subject"].ToString();
            string message = form["message"].ToString();

            MailMessage mail = new MailMessage();
            mail.To.Add("dharanarola2012@gmail.com");
            mail.From = new MailAddress(email);
            mail.Subject = subject;
            string Body = "You Got Message From: " + name + "This is the Message:" + message;
            mail.Body = Body;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "dharanarola2012@gmail.com",  // replace with valid value
                    Password = "K@09neeSUR"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mail);
                return RedirectToAction("HomePage");
            }


        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Shop()
        {
            return View();
        }
        public IActionResult Wishlist()
        {
            return View();
        }
        public IActionResult Cart()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            var productContext = _context.Cart.Include(c => c.Customer).Include(c => c.Product).Where(p => p.CustomerId == Convert.ToInt32(HttpContext.Session.GetString("CustomerID")));
            ViewData["Total"] = productContext.Sum(p => p.Product.PricePerCostUnit);
            var customerContext = _context.Customers.Where(p => p.CustomerId == Convert.ToInt32(HttpContext.Session.GetString("CustomerID"))).FirstOrDefault();
            ViewData["Fname"] = customerContext.CustomerFirstName;
            ViewData["LName"] = customerContext.CustomerLastName;
            ViewData["City"] = customerContext.City;
            ViewData["Address"] = customerContext.Address;
            ViewData["PostalCode"] = customerContext.PostalCode;
            ViewData["Phone"] = customerContext.CustomerPhone;
            ViewData["Email"] = customerContext.CustomerEmail;


            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private bool AdminExists(int id)
        {
             return _context.Admin.Any(e => e.AdminId == id);
        }
    }
}
