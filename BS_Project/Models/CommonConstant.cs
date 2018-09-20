using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BS_Project.Models
{
    public class CommonConstant
    {
        public static string cartSession = "CartSession";

        /// <summary>
        /// định dạng tiền việt nam
        /// </summary>
        /// <param name="money">số tiền truyền vào</param>
        /// <param name="strFormart">dấu muốn định nghĩa</param>
        /// <returns>VD: 1.000.000 VNĐ</returns>
        public string formatMoney(string money, string strFormart)
        {

            StringBuilder strB = new StringBuilder(money);
            int j = 0;
            string str = "";
            for (int i = strB.Length - 1; i > 0; i--)
            {
                j++;
                if (j % 3 == 0)
                {
                    strB.Insert(i, strFormart);
                    j = 0;
                }
            }

            str = strB.ToString();
            return str;
        }
    }
}