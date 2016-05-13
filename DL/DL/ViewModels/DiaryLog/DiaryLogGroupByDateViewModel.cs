using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.DiaryLog
{
    public class DiaryLogGroupByDateViewModel
    {
        public DateTime DiaryLogDate { get; set; }

        public List<Models.DiaryLog> DiaryLogs { get; set; }
    }
}