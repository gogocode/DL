using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DL.Web.Utilities
{
    public class Common
    {
        /// <summary>
        /// 取得年
        /// </summary>
        /// <param name="startYear"></param>
        /// <returns></returns>
        public static List<SelectListItem> GetYears(int startYear = 2016)
        {
            List<SelectListItem> years = new List<SelectListItem>();
            DateTime nowDate = System.DateTime.Now;
            int nowYear = nowDate.Year;

            for (int i = startYear; i <= nowYear; i++)
            {
                years.Add(new SelectListItem { Text = i.ToString() + " 年", Value = i.ToString() });
            }

            return years;
        }
    }
}