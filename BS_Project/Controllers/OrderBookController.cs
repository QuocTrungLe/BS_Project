using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BS_Project.Models;
using System.Web.Script.Serialization;
using BS_Project.DAO;

namespace BS_Project.Controllers
{
    public class OrderBookController : Controller
    {
        // GET: OrderBook
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Action sẽ thực hiện việc load số sách mà user đã đặt trong Order khi mới đăng nhập vào
        /// </summary>
        /// <returns></returns>
        //public JsonResult LoadOrderCart()
        //{
        //    int bookBorrowed = 0;

        //    //bookBorrowed = new OrderBookDAO().CountBookBuyed((Int32)Session["userID"]);
        //    if (Session["OrderCart"] == null)
        //    {
        //        Session["BookBorrowed"] = bookBorrowed;
        //    }
        //    else
        //    {
        //        int ssCart = (int)Session["OrderCart"];
        //        Session["BookBorrowed"] = Session["OrderCart"];
        //    }
        //    bookBorrowed = (Int32)Session["BookBorrowed"];
        //    return Json(bookBorrowed);
        //}

    }
}