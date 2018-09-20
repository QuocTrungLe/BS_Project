using BS_Project.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Project.DAO
{
    public class OrderBookDAO
    {
        private BSDBContext db = new BSDBContext();
        public List<int> GetOrderID(int userID)
        {
            return db.OrdersBooks.Where(x => x.UserID == userID).Select(x => x.OrderID).ToList();
        }

        public List<int> GetBookID(int orderID)
        {
            return db.OrdersDetails.Where(x => x.OrderID == orderID).Select(x => x.BookID).ToList();
        }

        public int GetID()
        {
            return db.OrdersBooks.Max(x => x.OrderID);
        }

        /// <summary>
        /// Đếm số lượng sách mà user đã mua
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int CountBookBuyed(int userID)
        {
            var bookBuyed = 0;
            //Lấy danh sách Order mà User đã mượn
            var orderID = GetOrderID(userID);
            var listBookID = GetBookID(Convert.ToInt32(orderID));
            foreach (var item in listBookID)
            {
                //Lấy danh sách chi tiết các sách mà User đã mua trong 1 Order
                bookBuyed++;
            }
            return bookBuyed;
        }

        public bool Insert(OrdersBook order)
        {
            try
            {
                string qrInsert = "Insert into OrdersBooks (FoundedDate, UserID, Status, Address, Phone, Email) Values ('" +
                    order.FoundedDate + "', " + order.UserID + ", " +
                     order.Status + ", N'" + order.Address + "', '" + order.Phone + "', '" + order.Email + "')"; 
                db.Database.ExecuteSqlCommand(qrInsert);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public IEnumerable<OrdersBook> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<OrdersBook> model = db.OrdersBooks;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = db.OrdersBooks.Where(x => x.User.Username.Contains(searchString));
            }
            return model.OrderBy(x => x.OrderID).ToPagedList(page, pageSize);
        }
    }
}