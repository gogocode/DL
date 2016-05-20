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
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime DiaryLogDate { get; set; }

        public ICollection<DiaryLogDetailVM> DiaryLogs { get; set; }
    }
}