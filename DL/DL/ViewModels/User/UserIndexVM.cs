using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.User
{
    public class UserIndexVM
    {
        public UserIndexVM()
        {
            Page = 0;
        }

        public IPagedList<Models.User> Users { get; set; }  // 符合條件資料

        /// <summary>
        /// 查詢條件 員工編號
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 查詢條件 姓名
        /// </summary>
        public string UserName { get; set; }

        public int Page { get; set; } //頁碼

    }
}