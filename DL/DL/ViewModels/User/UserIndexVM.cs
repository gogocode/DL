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

        public string UserAccount { get; set; }

        public int Page { get; set; } //頁碼

    }
}