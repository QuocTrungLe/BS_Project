using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Project.DAO
{
    class OrderBookDAO
    {
        BSDBContext db = new BSDBContext();
        public List<int> GetOrderID(int userID)
        {
            return db.OrdersDetail.Where(x => x.UserID == userID).Select(x => x.OrderID).ToList();
        }

        public List<int> GetBookID(int orderID)
        {
            return db.ListOfOrders.Where(x => x.OrderID == orderID).Select(x => x.BookID).ToList();
        }

        /// <summary>
        /// Đếm số lượng sách mà user đã mua
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int CountBookBuyed(int userID)
        {
            var bookBorrowed = 0;
            //Lấy danh sách Order mà User đã mượn
            var orderID = GetOrderID(userID);
            var listBookID = GetBookID(Convert.ToInt32(orderID));
            foreach (var item in listBookID)
            {
                //Lấy danh sách chi tiết các sách mà User đã mượn trong 1 Order
                bookBorrowed++;
            }
            return bookBorrowed;
        }
    }
}
