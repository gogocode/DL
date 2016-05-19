using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.User
{
    public class UserDeleteVM
    {
        [Key]
        public int UserId { get; set; }

        [Display(Name = "員工編號")]
        public string UserAccount { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "電子郵件")]
        public string UserEmail { get; set; }
    }
}