using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.DiaryLogNew
{
    public class DiaryLogNewIndexVM
    {
        public DiaryLogNewIndexVM()
        {
            Page = 0;
        }

        public DateTime? dateStart { get; set; }  // 搜尋條件1

        public DateTime? dateEnd { get; set; }  // 搜尋條件2

        public int UserId { get; set; }

        public IPagedList<DateTime> DiaryLogDate { get; set; }  // 符合條件資料

        public int Page { get; set; } //頁碼
    }
}