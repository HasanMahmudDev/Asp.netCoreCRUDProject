using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PublisherAndBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublisherAndBook.Controllers
{
    public class HomeController : Controller
    {
        readonly ManageDbContext db = null;

        readonly IWebHostEnvironment env;
        public HomeController(ManageDbContext db, IWebHostEnvironment env) { this.db = db; this.env = env; }
        public IActionResult Index()
        {
            ViewBag.PublisherCount = db.Publishers.Count();
            return View(db.Books.ToList());
        }
    }
}
