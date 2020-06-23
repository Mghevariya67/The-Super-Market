using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheIndianSuperMarket.Models;

namespace TheIndianSuperMarket.Controllers
{
    public class AdminsController : Controller
    {
        private readonly TheIndianMarketContext _context;
        string[] itemsName;
        string[] itemsQty;
        string[] itemsPrice;
        public AdminsController(TheIndianMarketContext context)
        {
            _context = context;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Admin.ToListAsync());
        }
        public IActionResult Administrator()
        {
            var AdminContext = _context.Admin.OrderBy(p=>p.Name);
            return View(AdminContext);
        }
        public IActionResult Customers()
        {
            var CustomerContext = _context.Customers.OrderBy(p => p.CustomerFirstName);
            return View(CustomerContext);
        }
        public IActionResult Products()
        {
            var ProductContext = _context.Products.OrderBy(p => p.DepartmentName);
            return View(ProductContext);
        }
        public IActionResult CustomerOrders()
        {
            var OrderContext = _context.CustomerOrder;
       
            return View(OrderContext);
        }
        public IActionResult StoreOrders()
        {
            return View();
        }
        public IActionResult Supplier()
        {
            var SupplierContext = _context.Suppliers.OrderBy(p => p.SupplierName);
            return View(SupplierContext);
        }
        public IActionResult SupplierProduct(int id)
        {
            var supplierContext = _context.Suppliers.Where(p => p.SupplierId == id).FirstOrDefault();
            var productContext = _context.Products.Include(p=>p.Supplier).Where(p=>p.SupplierId == id);
            ViewData["SupplierName"] = supplierContext.SupplierName;
            return View(productContext);
        }
        public IActionResult ProductCategories()
        {
            var DepartmentContext = _context.Departments.OrderBy(p => p.DepartmentName);
            return View(DepartmentContext);
        }
        public IActionResult Confirm(IFormCollection form, string SupplierName)
        {
            var supplierProductContext = _context.Suppliers.Where(p => p.SupplierName == SupplierName).FirstOrDefault();
            var productContext = _context.Products.Where(p => p.SupplierId == supplierProductContext.SupplierId);
            itemsName = new string[productContext.Count()];
            itemsQty = new string[productContext.Count()];
            itemsPrice = new string[productContext.Count()];
            int i = 0;
            foreach (var item in productContext)
            {
                itemsName[i] = item.ProductName;
                itemsQty[i] = Request.Form[item.ProductName + "+QTY"];
                itemsPrice[i] = Request.Form[item.ProductName];
                i++;
            }

            
            /*********for generatin pdf************/
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = new DateTime();
            //file name to be created   
            string strPDFFileName = string.Format("ProductCatelog" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table with 5 columns  
            PdfPTable tableLayout = new PdfPTable(4);
            doc.SetMargins(0f, 0f, 0f, 0f);
            //Create PDF Table  

            //file will created in this path  
            var strAttachment = new FileStream(Path.Combine("C:\\Users\\Dhara Narola\\", strPDFFileName), FileMode.Create);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   
            doc.Add(Add_Content_To_PDF(tableLayout, SupplierName));
            
            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            File(workStream, "application/pdf", strPDFFileName);
           
                /******************/
                MailMessage mail = new MailMessage();
            mail.To.Add("dharanarola2012@gmail.com");
                mail.From = new MailAddress("abc@gmail.com");
                mail.Subject = "Message From Vivek Shah Regarding To Order";
                string Body = " \n" +
                    "Please Kindly Check order detail for next order!\n" +
                    "SupplierName: " + SupplierName + "\n" +
                    ""
                    ;
                string mainBody = null;

                for (int k = 0; k < itemsName.Count(); k++)
                {
                    mainBody += " " + itemsName[k] + ": " + itemsQty[k] + " " + itemsPrice[k] + "\n";
                }
                Body += mainBody;
                mail.Body = Body;
             
            //mail.Attachments.Add(new Attachment("C:\\Users\\Dhara Narola", strPDFFileName));
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
                    TempData["Message"] = "Order Placed Succesfully!";
                    RedirectToAction("Supplier");

            }
           return File(workStream, "application/pdf", strPDFFileName);

        }


        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout, string name)
        {
            float[] headers = { 15, 15, 15,15 }; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 80; //Set the PDF File witdh percentage  
            tableLayout.HeaderRows = 1;
            //Add Title to the PDF file at the top  
            tableLayout.AddCell(new PdfPCell(new Phrase("Purchase Order", new Font(Font.FontFamily.HELVETICA, 12, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("Date: " + DateTime.Now, new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("The Indian Super Market", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("509 Wilson Ave #18", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("Kitchener, ON, CA" , new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("N2C 2M4" , new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
         
         
            tableLayout.AddCell(new PdfPCell(new Phrase("Vendor: " + name, new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            ////Add header  
            AddCellToHeader(tableLayout, "Product Name");
            AddCellToHeader(tableLayout, "Quantity");
            AddCellToHeader(tableLayout, "Price");
            AddCellToHeader(tableLayout, "Total");


            ////Add body  
            var massTotal = 0.00;
            for (int p = 0; p < itemsName.Count(); p++)
            {
                var Total = Convert.ToDouble(itemsPrice[p]) * Convert.ToInt32(itemsQty[p]);
                massTotal += Total;
                AddCellToBody(tableLayout, itemsName[p]);
                AddCellToBody(tableLayout, itemsQty[p]);
                AddCellToBody(tableLayout, itemsPrice[p]);
                AddCellToBody(tableLayout, Total.ToString());
            }
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("ShipTo: Vinit Patel (The Indian Super Market)", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
           
            
            tableLayout.AddCell(new PdfPCell(new Phrase("Discount : 0.00", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("Tax : 0.00", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("----------------", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("Total: " + massTotal, new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("Please read and accept the catelog from The Indian Super Market. By sigining this catelog, you accepted the Product Price on the date it was sended to you.", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("____________________", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            tableLayout.AddCell(new PdfPCell(new Phrase("(Sign Here)", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 10,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            });
            return tableLayout;

        }

            private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
            {

                tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
                 });
             }
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
             });
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(int? id)
       {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,Name,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdminId,Name,Password")] Admin admin)
        {
            if (id != admin.AdminId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.AdminId))
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
            return View(admin);
        }

        /* GET: Admins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admin
                .FirstOrDefaultAsync(m => m.AdminId == id);
            if (admin == null)
            {
                return NotFound();
            }

            return View(admin);
        }*/

        // POST: /Delete/5
        
        public async Task<IActionResult> Delete(int id)
        {

            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            TempData["Message"] = products.ProductName + " has been deleted Successfully!";
            return RedirectToAction(nameof(Products));
        }

        private bool AdminExists(int id)
        {
            return _context.Admin.Any(e => e.AdminId == id);
        }
    }
}
