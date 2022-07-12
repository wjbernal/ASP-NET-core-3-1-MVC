using BookListMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookListMVC.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDBContext _db;

        [BindProperty]
        public Book Book { get; set; }

        public BooksController(ApplicationDBContext db)
        {
            _db = db;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Book = new Book();

            if (id == null)
            {
                // this is for create a new book
                // it will return an empty Book
                return View(Book);
            }
            else
            {
                Book = _db.Books.FirstOrDefault(b => b.Id == id);
                return View(Book);
            }        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert()
        {
            if (ModelState.IsValid)
            {
                // creating a new Book
                if(Book.Id == 0)
                {
                    _db.Books.Add(Book);
                }
                // it is for update
                else
                {
                    _db.Books.Update(Book);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(Book);
        }

        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bookFromDb = await _db.Books.FirstOrDefaultAsync(i => i.Id == id);

            if (bookFromDb == null)
            {
                return Json(new { success = false, messageResult = "Error selecting a Book to be deleted" });
            }

            _db.Books.Remove(bookFromDb);
            await _db.SaveChangesAsync();
            return Json(new { success = true, messageResult = "Book Deleted successfuly!" });

        }
        #endregion


    }
}
