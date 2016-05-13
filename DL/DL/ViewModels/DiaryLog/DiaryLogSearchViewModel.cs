using DL.Models;
using DL.Web.ViewModels.DiaryLog;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DL.ViewModels.DiaryLog
{
    public class DiaryLogSearchViewModel
    {
        public DiaryLogSearchViewModel()
        {
            dateStart = null;
            dateEnd = null;
            content = string.Empty;
            Page = 0;
        }

        public DateTime? dateStart { get; set; }  // 搜尋條件1

        public DateTime? dateEnd { get; set; }  // 搜尋條件2

        public string content { get; set; }  // 搜尋條件3

        public IPagedList<DiaryLogGroupByDateViewModel> DiaryLogGroupByDates { get; set; }  // 符合條件資料

        public int Page { get; set; } //頁碼
    }
}