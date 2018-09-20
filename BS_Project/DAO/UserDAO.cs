using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class UserDAO
    {
        private BSDBContext db = new BSDBContext();

        public IQueryable<string> getUserID(int userID)
        {
            return db.Users.Where(x => x.UserID == userID).Select(x => x.Username).Distinct();
        }

        public string getName(int userID)
        {
            var a = db.Users.Where(x => x.UserID == userID).FirstOrDefault();
            return a.FullName;
        }

        public string getPhone(int userID)
        {
            var a = db.Customers.Where(x => x.UserID == userID).FirstOrDefault();
            return a.Phone;
        }

        public string getAddress(int userID)
        {
            var a = db.Customers.Where(x => x.UserID == userID).FirstOrDefault();
            return a.Address;
        }

        public string getEmail(int userID)
        {
            var a = db.Users.Where(x => x.UserID == userID).FirstOrDefault();
            return a.Email;
        }
    }
}