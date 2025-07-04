using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace WareHouseMVC.Common
{
    public class CommonFunctions
    {

        public static Int64 GenerateRandomNumber(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            string s;
            for (int i = 0; i < size; i++)
            {
                s = Convert.ToString(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(s);
            }

            return Convert.ToInt64((builder.ToString()));
        }

       
    }
}