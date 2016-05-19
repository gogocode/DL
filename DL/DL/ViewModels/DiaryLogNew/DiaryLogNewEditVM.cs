using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.DiaryLogNew
{
    public class DiaryLogNewEditVM
    {
        /// <summary>
        /// 返回列表傳值使用
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 顯示 員工編號
        /// </summary>
        public string UserAccount { get; set; }

        /// <summary>
        /// 顯示 姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 新增 時間
        /// </summary>
        public DateTime DiaryLogDate { get; set; }

        //public List<Models.DiaryLog> DiaryLogs {get;set;}

        public ICollection<DiaryLogDetailVM> DiaryLogs { get; set; }
    }
}