using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.DiaryLogNew
{
    public class DiaryLogNewCreateVM
    {
        public int UserId { get; set; }

        public string UserAccount { get; set; }

        public string UserName { get; set; }

        [Required]
        [Display(Name ="時間")]
        public DateTime DiaryLogDate { get; set; }

        //public List<Models.DiaryLog> DiaryLogs {get;set;}

        public ICollection<DiaryLogDetailVM> DiaryLogs { get; set; }
    }
}