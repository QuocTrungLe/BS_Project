using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS_Project.DAO
{
    public class OrdersDetailsDAO
    {
        private BSDBContext db = new BSDBContext();

        public bool Insert(OrdersDetail temp)
        {
            try
            {
                db.OrdersDetails.Add(temp);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}