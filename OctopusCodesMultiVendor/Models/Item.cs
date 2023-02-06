using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IDETicaret.Models
{
    public class Item
    {
        public Product product { get; set; }
        public int quantity { get; set; }
        public bool Aktarilacak { get; set; }
    }

    public class IDUtils
    {

        public static string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
    }
}