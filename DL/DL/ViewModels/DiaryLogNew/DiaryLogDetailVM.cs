using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.DiaryLogNew
{
    public class DiaryLogDetailVM
    {
        public int DiaryLogId { get; set; }
        public int UserId { get; set; }
        [Required]
        [Display(Name ="項目")]
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
        [Display(Name = "是否解決")]
        public string DiaryLogSolve { get; set; }
    }
}