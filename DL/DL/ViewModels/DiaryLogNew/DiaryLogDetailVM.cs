using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        [RegularExpression(@"^\d+.?\d{0,1}$", ErrorMessage = "最多1位小數")]
        [Range(0,99.9, ErrorMessage = "最大2位數字")]
        public decimal DiaryLogHours { get; set; }

        [Display(Name = "狀況")]
        public string DiaryLogSituation { get; set; }

        [Display(Name = "是否解決")]
        public string DiaryLogSolve { get; set; }

        public List<SelectListItem> DiaryLogItems { get; set; }
    }
}