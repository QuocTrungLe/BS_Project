using BS_Project.DAO;
using BS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BS_Project.EF;

namespace BS_Project.Controllers
{
    public class CartItemController : Controller
    {
        // GET: CartItem
        public ActionResult Index()
        {
            var cart = Session[CommonConstant.cartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        /// <summary>
        /// Thêm book vao giỏ hàng
        /// </summary>
        /// <param name="bookID"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public JsonResult AddItem(int bookID, int quantity)
        {
            var book = new BookDAO().BookDetail(bookID);
            var status = true;
            var cart = Session[CommonConstant.cartSession];
            var list = (List<CartItem>)cart;
            //đơn hàng đang được giao tới...
            int orderShipping = 0;
            if (Session["userID"] != null)
            {
                int userID = (Int32)Session["userID"];
                orderShipping = new CartItemDAO().CountBookBorrowed(userID);

                if (orderShipping < 5)
                {
                    //gán Session vào list
                    //list = (List<CartItem>)cart;
                    int totalBuy = orderShipping;
                    //Tổng số sách mượn
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            totalBuy += item.Quantity;
                        }
                    }
                    if (totalBuy < 10)
                    {
                        if (cart != null)
                        {
                            //Thêm Book vào giỏ hàng
                            if (list.Exists(x => x.Books.BookID == bookID))
                            {
                                foreach (var item in list)
                                {
                                    if (item.Books.BookID == bookID)
                                    {
                                        item.Quantity += quantity;
                                        item.TotalBook = totalBuy + 1;
                                    }
                                }
                            }
                            else
                            {
                                //Tạo mới giỏ hàng
                                var item = new CartItem();
                                item.Books = book;
                                item.Quantity = quantity;
                                item.TotalBook = totalBuy + 1;
                                //Thêm sản phẩm vào giỏ hàng
                                list.Add(item);
                            }
                            Session[CommonConstant.cartSession] = list;
                        }
                        else
                        {
                            //Tạo mới giỏ hàng
                            var item = new CartItem();
                            item.Books = book;
                            item.Quantity = quantity;
                            item.TotalBook = totalBuy + 1;
                            //Thêm sản phẩm vào giỏ hàng
                            list = new List<CartItem>();
                            list.Add(item);

                            //Gán lại cart session user đã đặt rồi
                            Session[CommonConstant.cartSession] = list;
                        }
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                else
                {
                    //Xuất ra thông báo số lượng sách bạn đã mua vượt quá 10
                }
            }
            else
            {
                //Xuất ra thông báo chưa đăng nhập
            }
            return Json(new
            {
                totalBuy = list[list.Count - 1].TotalBook,
                Status = status
            });
            //return Json(temp);
        }

        /// <summary>
        /// Action Update Order Cart sẽ tiến hành Update giỏ hàng được gọi lên từ cartController
        /// </summary>
        /// <param name="cartModel">List Book cần Update</param>
        /// <returns></returns>
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CommonConstant.cartSession];

            foreach (var item in sessionCart)
            {

                var jsonItem = jsonCart.SingleOrDefault(x => x.Books.BookID == item.Books.BookID);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CommonConstant.cartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        /// <summary>
        /// Xóa tất cả giỏ hàng
        /// </summary>
        /// <returns></returns>
        public JsonResult DeleteAll()
        {
            Session[CommonConstant.cartSession] = null;
            return Json(new
            {
                status = true
            });
        }

        // <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteByID(int id)
        {
            var sessionCart = (List<CartItem>)Session[CommonConstant.cartSession];
            sessionCart.RemoveAll(x => x.Books.BookID == id);
            Session[CommonConstant.cartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        [HttpGet]
        public ActionResult Payment()
        {
            var cart = Session[CommonConstant.cartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Payment(string CustName, string CustPhone, string CustAdd, string CustEmail)
        {
            var orderBook = new OrdersBook();
            int userID = Convert.ToInt32(Session["userID"]);
            //Lưu vào trong db OrderBook
            orderBook.FoundedDate = DateTime.Now;
            orderBook.UserID = userID;
            orderBook.Status = 0; //đơn hàng vừa được đặt sẽ có trạng thái là đang giao
            orderBook.Address = CustAdd;
            orderBook.Phone = CustPhone;
            orderBook.Email = CustEmail;
            
            var id = new OrderBookDAO().Insert(orderBook);
            int tempID = new OrderBookDAO().GetID();
            try
            {
                var cart = (List<CartItem>)Session[CommonConstant.cartSession];
                var detailDAO = new OrdersDetailsDAO();
                foreach (var item in cart)
                {
                    var orderDetail = new OrdersDetail();
                    orderDetail.OrderID = tempID;
                    orderDetail.BookID = item.Books.BookID;
                    orderDetail.Total = (item.Quantity * item.Books.Price);
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.CreatedDate = DateTime.Now;
                    orderDetail.Status = 1;
                    detailDAO.Insert(orderDetail);
                }
                Session[CommonConstant.cartSession] = null;
            }
            catch (Exception ex)
            {
                throw;
            }
            return Redirect("/hoan-thanh");
        }

        public ActionResult Succeed()
        {
            return View();
        }
    }
}