using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.DiaryLogNew
{
    public class DiaryLogNewEditViewModel
    {
        /// <summary>
        /// 返回列表傳值使用
        /// </summary>
        public int UserId { get; set; }

        public string UserAccount { get; set; }

        public string UserName { get; set; }

        public DateTime DiaryLogDate { get; set; }

        //public List<Models.DiaryLog> DiaryLogs {get;set;}

        public ICollection<Models.DiaryLog> DiaryLogs { get; set; }
    }
}