using System.Diagnostics;
using book.Models;
using book.Services;
using Microsoft.AspNetCore.Mvc;

namespace book.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController>? _logger;
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public HomeController(ApplicationDbContext context, IWebHostEnvironment environment, ILogger<HomeController>? logger = null)
        {
            this.context = context;
            this.environment = environment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var books = context.Books.ToList();
            return View(books);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Books books)
        {
            if (books.ImgFile == null)

            {
                ModelState.AddModelError("ImgFileName", "Product image is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(books);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(books.ImgFile!.FileName);

          string imageFullPath = Path.Combine(environment.WebRootPath, "books", newFileName);


            using (var stream = System.IO.File.Create(imageFullPath))
            {
                books.ImgFile.CopyTo(stream);
            }
            books.ImgFileName = newFileName;

            Books newBook = new Books()

            {
                Title = books.Title,
                Author = books.Author,
                Genre = books.Genre,
                Description = books.Description,
                ImgFileName = newFileName,
            };
            //Console.WriteLine("ModelState Valid: " + ModelState.IsValid);

            context.Books.Add(books);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Books updatedBook)
        {
            var bookFromDb  = context.Books.Find(updatedBook.Id);

            if (bookFromDb == null)
            {
                return NotFound();
            }

            if (updatedBook.ImgFile != null)
            {
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(updatedBook.ImgFile.FileName);

                string imagePath = Path.Combine(environment.WebRootPath, "books", newFileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    updatedBook.ImgFile.CopyTo(stream);
                }
                if (!string.IsNullOrEmpty(bookFromDb.ImgFileName))
                {
                    string oldImagePath = Path.Combine(environment.WebRootPath, "books", bookFromDb.ImgFileName);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                bookFromDb.ImgFileName = newFileName;
            }
            bookFromDb.Title = updatedBook.Title;
            bookFromDb.Author = updatedBook.Author;
            bookFromDb.Genre = updatedBook.Genre;
            bookFromDb.Description = updatedBook.Description;
            bookFromDb.ReleaseDate = updatedBook.ReleaseDate;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var books = context.Books.Find(id);

            if (books == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if(!string.IsNullOrEmpty(books.ImgFileName))
            {
                string imageFullPath = Path.Combine(environment.WebRootPath, "books", books.ImgFileName);


                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }
                        }


            context.Books.Remove(books);
            context.SaveChanges();
            return RedirectToAction("Index", "Home");
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
    }
}
