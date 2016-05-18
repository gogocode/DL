using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.DiaryLogNew
{
    public class DiaryLogNewMasterIndexVM
    {
        public DiaryLogNewMasterIndexVM()
        {
            Page = 0;
        }

        /// <summary>
        /// 查詢條件 會員編號
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 查詢條件 會員姓名
        /// </summary>
        public string UserName { get; set; }

        public IPagedList<UserGroupVM> UserGroups { get; set; }  // 符合條件資料

        public int Page { get; set; } //頁碼
    }
}