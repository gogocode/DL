using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.Utilities.Attribute
{
    public class EmailAttribute : RegularExpressionAttribute
    {
        public EmailAttribute()
            : base(Regex())
        { }

        private static string Regex()
        {
            //@86shop.com.tw
            return @"^\w+@86shop.com.tw$";
        }
    }
}