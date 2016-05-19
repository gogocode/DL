using System;
using System.ComponentModel.DataAnnotations;

namespace DL.ViewModels.DiaryLog
{
    public class DiaryLogViewModel
    {
        public int DiaryLogId { get; set; }

        public int UserId { get; set; }

        [Required]
        [Display(Name = "日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DiaryLogDate { get; set; }

        [Required]
        [Display(Name = "工作項目")]
        public string DiaryLogItem { get; set; }

        [Required]
        [Display(Name = "內容")]
        public string DiaryLogContents { get; set; }

        [Required]
        [Display(Name = "狀態")]
        public bool DiaryLogStatus { get; set; }

        [Required]
        [Display(Name = "時數")]
        public int DiaryLogHours { get; set; }

        [Display(Name = "狀況")]
        public string DiaryLogSituation { get; set; }

        [Display(Name = "如何解決")]
        public string DiaryLogSolve { get; set; }

        public string CreateId { get; set; }

        public Nullable<System.DateTime> CreateDate { get; set; }

        public string UpdateId { get; set; }

        public Nullable<System.DateTime> UpdateDate { get; set; }


    }
}