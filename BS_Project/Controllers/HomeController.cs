using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BS_Project.DAO;
using System.Web.Mvc;
using BS_Project.EF;
using BS_Project.Security;
using BS_Project.Models;
using System.Data.SqlClient;

namespace BS_Project.Controllers
{
    public class HomeController : Controller
    {
        private BSDBContext db = new BSDBContext();
        CommonConstant cmCon = new CommonConstant();
        RemoveSignVNModel rmS = new RemoveSignVNModel();
        public ActionResult Index()
        {
            List<Book> rs = db.Books.Take(8).ToList();
            ViewData["listHot"] = rs;
            List<Book> xs = db.Books.Take(4).OrderByDescending(x => x.PublicationDate).ToList();
            ViewData["listNew"] = xs;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public PartialViewResult Publisher()
        {
            List<Publisher> list = db.Publishers.ToList<Publisher>();
            return PartialView(list);
        }

        public PartialViewResult Categories()
        {
            //List<Category> list = Context.Categories.ToList<Category>();
            //return PartialView(list);
            List<Category> list = db.Categories.ToList();
            ViewData["List"] = list;
            return PartialView(list);
        }

        [HttpGet] 
        public ActionResult Login()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Login(User ac)
        {
            var result = db.Users.Where(p => p.Username == ac.Username && p.Password == ac.Password).Count();
            if(result == 0)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại hoặc Tên đăng nhập, mật khấu bị sai.");
            }
            else
            {
                var userID = db.Users.Where(a => a.Username == ac.Username).FirstOrDefault().UserID;
                var name = db.Users.Where(a => a.Username == ac.Username && a.Password == ac.Password).FirstOrDefault().Username;
                var blockID = db.Users.Where(a => a.Username == ac.Username).FirstOrDefault().isActivated;
                var roleID = db.Users.Where(a => a.Username == ac.Username).FirstOrDefault().UserRoleID;
                Session["userHello"] = name;
                Session["userName"] = ac;
                Session["UserID"] = userID;
                Session["blockID"] = blockID;
                Session["roleID"] = roleID;
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Register(User account)
        {
            if (account.Username == null || account.Password == null)
            {
                return View();
            }
            if(ModelState.IsValid)
            {
                using (BSDBContext db = new BSDBContext())
                {
                    var user = db.Users.FirstOrDefault(p => p.Username == account.Username);
                    if (user == null)
                    {
                        account.Password = HashPassword.getHash(account.Password);
                        db.Users.Add(account);
                        db.SaveChanges();
                    }
                    else{
                        ViewBag.Message = "UserName already exists" + account.Username;
                        return View();
                    }
                }
                ModelState.Clear();
                ViewBag.Message = "Successfully Registered Mr. " + account.Username;
            }
            return RedirectToAction("Index");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        public string FormatMoney(string money)
        {
            return cmCon.formatMoney(money, ".");
        }

        /// <summary>
        /// Ham BookDetail co tham so truyen vao la id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult BookDetail(int id)
        {
            var model = db.Books.Find(id);
            // Tăng số lần xem
            model.ViewCount++;
            db.SaveChanges();
            // Lấy cookie cũ tên views
            var views = Request.Cookies["views"];
            // Nếu chưa có cookie cũ -> tạo mới
            if (views == null)
            {
                views = new HttpCookie("views");
            }
            // Bổ sung mặt hàng đã xem vào cookie
            views.Values[id.ToString()] = id.ToString();
            // Đặt thời hạn tồn tại của cookie
            views.Expires = DateTime.Now.AddMonths(1);
            // Gửi cookie về client để lưu lại
            Response.Cookies.Add(views);
            // Lấy List<int> chứa mã hàng đã xem từ cookie
            var keys = views.Values.AllKeys.Select(k => int.Parse(k)).ToList();
            // Truy vấn hàng đã xem
            ViewBag.Views = db.Books.Where(p => keys.Contains(p.BookID));
            return PartialView(model);

            //// trả về tất cả các trường trong sách có id là tham số truyền vào
            //var model = Context.Books.Where(p => p.BookID == id).SingleOrDefault();
            //return PartialView(model);
        }

        [ChildActionOnly]
        public PartialViewResult HeaderCart()
        {
            var cart = Session[CommonConstant.cartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            ViewBag.Status = Session["userID"];
            ViewBag.BookBorrowed = Session["BookBorrowed"];

            return PartialView(list);
        }
        public ActionResult Logout()
        {
            Session["userName"] = null;
            Session["userID"] = null;
            Session["OrderCart"] = null;
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult Search(string keysearch = "")
        {
            //return View("~/Views/Home/Index.cshtml");

            return RedirectToAction("ShowBook", "Book", new { condition = 4, keysearch = keysearch });
        }

        [HttpPost]
        public JsonResult SearchAutoComplete(string keySearch = "")
        {
            List<string> _json = new List<string>();
            string removeUnicodeKeySearch = rmS.RemoveSign4VietnameseString(keySearch);

            var allBooks = db.Database.SqlQuery<Book>("ShowBookFollowKeySearch @keySearch",
                new SqlParameter("@keySearch", removeUnicodeKeySearch)).ToList();
            // lấy ra 1 list danh sách tên các sách
            foreach (var item in allBooks)
            {
                _json.Add(item.Name);
            }

            return Json(_json);
        }
    }
}