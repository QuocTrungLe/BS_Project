using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BS_Project.DAO
{
    public class BookDAO
    {
        public BSDBContext db = new BSDBContext();

        public Book BookDetail(int id)
        {
            return db.Books.Find(id);
        }

        /// <summary>
        /// Update thông tin của 1 cuốn sách
        /// </summary>
        /// <param name="book"></param>
        /// <param name="imageURL"></param>
        /// <returns></returns>
        public bool Update(Book book, string imageURL)
        {
            try
            {
                var bookOld = db.Books.Find(book.BookID);
                bookOld.ImageBool.Images.First().ImageURL = imageURL;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        // <summary>
        /// Thêm 1 cuốn sách
        /// </summary>
        /// <param name="book"></param>
        /// <param name="imageURL"></param>
        /// <returns>trả về ID của cuốn sách được thêm</returns>
        public int Insert(Book book, string imageURL)
        {
            db.Books.Add(book);
            var imageBool = new ImageBool();
            var image = db.ImageBools.Add(imageBool);
            db.SaveChanges();

            string qrInsertImage = "insert into Images values (" + image.ImageBoolID + ", N'" + imageURL + "');";
            string updateBookImage = "update Books set ImageBoolID = " + image.ImageBoolID + " where BookID = " + book.BookID;
            db.Database.ExecuteSqlCommand(qrInsertImage);
            db.Database.ExecuteSqlCommand(updateBookImage);

            db.SaveChanges();
            return book.BookID;
        }
    }
}