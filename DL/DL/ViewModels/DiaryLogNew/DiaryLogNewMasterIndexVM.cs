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

        public IPagedList<UserGroupVM> UserGroups { get; set; }  // 符合條件資料

        public int Page { get; set; } //頁碼
    }
}