using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class AccountDAO
    {
        private BSDBContext db = new BSDBContext();

        public bool Insert(User ac)
        {
            try
            {
                db.Users.Add(ac);
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