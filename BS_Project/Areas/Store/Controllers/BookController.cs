﻿using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using PagedList;
using BS_Project.EF;
using BS_Project.DAO;

namespace BS_Project.Areas.Store.Controllers
{
    public class BookController : Controller
    {
        // GET: Store/Book
        private BSDBContext db = new BSDBContext();

        /// <summary>
        /// Hiện thị thông tin của tất cả cuốn sách trên trang index
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Index(string searchString, int page = 1, int pageSize = 3)
        {
            IQueryable<Book> books = db.Books.Include(b => b.ImageBool).Include(b => b.Publisher);
            if (!string.IsNullOrEmpty(searchString))
            {
                books = books.Where(x => x.Name.Contains(searchString) || x.Publisher.Name.Contains(searchString) || x.Authors.FirstOrDefault().Name.Contains(searchString));
            }
            ViewBag.SearchString = searchString;
            return View(books.OrderBy(x => x.BookID).ToPagedList(page, pageSize));
        }

        /// <summary>
        /// Hiện thị trang sửa 1 cuốn sách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageBoolID = new SelectList(db.ImageBools, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", book.Authors.First().AuthorID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name", book.PublisherID);
            return View(book);
        }

        /// <summary>
        /// Sửa 1 cuốn sách và lưu nó vào database
        /// </summary>
        /// <param name="book"></param>
        /// <param name="formcollection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book, FormCollection formcollection)
        {
            string imageURL = null;
            try
            {
                imageURL = formcollection["txtImageURL"].ToString();
            }
            catch
            {
                imageURL = "/Content/images/Image.jpg";
            }
            if (ModelState.IsValid)
            {
                var dao = new BookDAO();
                var rs = dao.Update(book, imageURL);
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ImageBoolID = new SelectList(db.ImageBools, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", book.Authors.First().AuthorID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name", book.PublisherID);
            return View(book);
        }

        /// <summary>
        /// Hiện thị thông tin chi tiết của 1 cuốn sách chọn theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        /// <summary>
        /// hiện thị trang tạo mới 1 cuốn sách
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ViewBag.ImageBoolID = new SelectList(db.ImageBools, "ImageBoolID", "ImageBoolID");
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name");
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name");
            return View();
        }

        /// <summary>
        /// Tạo mới 1 cuốn sách và lưu nó lại
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Book book, FormCollection formcollection)
        {
            string imageURL = null;
            try
            {
                imageURL = formcollection["txtImageURL"].ToString();
            }
            catch
            {
                imageURL = "/Content/images/Image.jpg";
            }
            if (ModelState.IsValid)
            {

                var dao = new BookDAO();
                var rs = dao.Insert(book, imageURL);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ImageBoolID = new SelectList(db.ImageBools, "ImageBoolID", "ImageBoolID", book.ImageBoolID);
            //ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", book.Authors.First().AuthorID);
            ViewBag.PublisherID = new SelectList(db.Publishers, "PublisherID", "Name", book.PublisherID);
            return View(book);
        }
    }
}