using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublisherAndBook.Models;
using PublisherAndBook.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PublisherAndBook.Controllers
{
    public class BooksController : Controller
    {
        readonly ManageDbContext db = null;

        readonly IWebHostEnvironment env;
        public BooksController(ManageDbContext db, IWebHostEnvironment env) { this.db = db; this.env = env; }

        public IActionResult Index(int pg = 1)
        {
            int totalPages = (int)Math.Ceiling((double)db.Books.Count() / 3);
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = pg;
            var data = db.Books.Include(x => x.Publisher).OrderBy(x => x.BookId).Skip((pg - 1) * 3).Take(3).ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            ViewBag.Publishers = db.Publishers.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(BookCreateModel book)
        {
            if (ModelState.IsValid)
            {
                var newBook = new Book
                {
                    Picture = "demo.jpg",
                    BookName = book.BookName,
                    Price = book.Price,
                    Status = book.Status,
                    PublisherId = book.PublisherId
                };
                if (book.Picture != null && book.Picture.Length > 0)
                {
                    string dir = Path.Combine(env.WebRootPath, "Uploads");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fileName = Guid.NewGuid() + Path.GetExtension(book.Picture.FileName);
                    string fullPath = Path.Combine(dir, fileName);
                    FileStream fs = new FileStream(fullPath, FileMode.Create);
                    book.Picture.CopyTo(fs);
                    fs.Flush();
                    newBook.Picture = fileName;
                }
                db.Books.Add(newBook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Publishers = db.Publishers.ToList();
            return View(book);
        }

        public IActionResult Edit(int Id)
        {
            ViewBag.Publishers = db.Publishers.ToList();
            var book = db.Books.Include(p => p.Publisher).First(b => b.BookId == Id);
            ViewBag.CurrentPicture = book.Picture;
            return View(new BookCreateModel
            {
                BookId = book.BookId,
                BookName = book.BookName,
                Price = book.Price,
                Status = book.Status,
                PublisherId = book.PublisherId
            });
        }
        [HttpPost]
        public IActionResult Edit(BookCreateModel book)
        {
            var bookExists = db.Books.First(b => b.BookId == book.BookId);
            if (ModelState.IsValid)
            {
                bookExists.BookName = book.BookName;
                bookExists.Price = book.Price;
                bookExists.Status = book.Status;
                bookExists.PublisherId = book.PublisherId;
                if (book.Picture != null && book.Picture.Length > 0)
                {
                    string dir = Path.Combine(env.WebRootPath, "Uploads");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string fileName = Guid.NewGuid() + Path.GetExtension(book.Picture.FileName);
                    string fullPath = Path.Combine(dir, fileName);
                    FileStream fs = new FileStream(fullPath, FileMode.Create);
                    book.Picture.CopyTo(fs);
                    fs.Flush();
                    bookExists.Picture = fileName;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Publishers = db.Publishers.ToList();
            ViewBag.currentPicture = bookExists.Picture;
            return View(book);
        }
        public IActionResult Delete(int Id)
        {
            return View(db.Books.Include(b => b.Publisher).First(p => p.BookId == Id));
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DoDelete(int Id)
        {
            var Book = new Book { BookId = Id };
            db.Entry(Book).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
