using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "員工編號")]
        [MaxLength(4,ErrorMessage ="員工編號最多4位數")]
        public string AccountId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }
}