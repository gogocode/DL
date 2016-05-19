using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.User
{
    public class UserEditVM
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "員工編號")]
        public string UserAccount { get; set; }

        [Required]
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required]
        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = "密碼和確認密碼不相符。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "電子郵件")]
        public string UserEmail { get; set; }

        [DataType(DataType.Password)]
        public string OriginPassword { get; set; }
    }
}