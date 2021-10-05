using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublisherAndBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublisherAndBook.Controllers
{
    public class PublishersController : Controller
    {
        readonly ManageDbContext db;
        public PublishersController(ManageDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View(db.Publishers.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Publishers.Add(publisher);
                db.SaveChanges();
                return PartialView("_CreatePartial", true);
            }
            return PartialView("_CreatePartial", false);
        }
        //Edit
        public IActionResult Edit(int id)
        {
            return View(db.Publishers.First(x => x.PublisherId == id));

        }

        [HttpPost]
        public IActionResult Edit(Publisher p)
        {
            if (ModelState.IsValid)
            {
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("_EditPartial", true);
            }
            return PartialView("_EditPartial", false);
        }

        public IActionResult Delete(int id)
        {
            return View(db.Publishers.First(x => x.PublisherId == id));
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DoDelete(int id)
        {
            var publisher = new Publisher { PublisherId = id };
            if (!db.Books.Any(x => x.PublisherId == id))
            {
                db.Entry(publisher).State = EntityState.Deleted;
                db.SaveChanges();
                return PartialView("_PDelete", true);
            }
            ModelState.AddModelError("", "Cannot delete. Publisher has related Books.");
            //return View(db.Publishers.First(x => x.PublisherId == id));
            return PartialView("_PDelete", false);

        }
    }
}
