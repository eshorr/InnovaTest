// PDF generation using iText 7
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InnovaTest.Core.Data;
using InnovaTest.Core.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks; // For async operations

namespace InnovaTest.Core.Controllers
{
    public class PeopleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        // The DbContext and the Hosting Environment are "injected" by the framework.
        public PeopleController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: People (List and Search)
        public async Task<IActionResult> Index(string searchString)
        {
            var people = from p in _context.People
                         select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                people = people.Where(s => s.FullName.Contains(searchString));
            }

            return View(await people.ToListAsync());
        }

        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: People/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Person person)
        {
            if (ModelState.IsValid)
            {
                // File upload logic for .NET Core
                if (person.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(person.ImageFile.FileName);
                    string extension = Path.GetExtension(person.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    person.ImagePath = "/images/" + fileName; // Path to be stored in DB

                    string path = Path.Combine(wwwRootPath + "/images/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await person.ImageFile.CopyToAsync(fileStream);
                    }
                }

                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: People/ExportToPdf
        public async Task<IActionResult> ExportToPdf()
        {
            var people = await _context.People.OrderBy(p => p.FullName).ToListAsync();

            // iText 7 PDF generation logic
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = new PdfWriter(ms);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            document.Add(new Paragraph("People List").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20));

            Table table = new Table(3, true); // 3 columns

            table.AddHeaderCell("Full Name");
            table.AddHeaderCell("Phone Number");
            table.AddHeaderCell("Email");

            foreach (var person in people)
            {
                table.AddCell(person.FullName);
                table.AddCell(person.PhoneNumber ?? "");
                table.AddCell(person.Email);
            }

            document.Add(table);
            document.Close();

            // Return the PDF file
            return File(ms.ToArray(), "application/pdf", "PeopleList.pdf");
        }
    }
}