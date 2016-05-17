/*****************************************************************
* Date: 2016/05/11
* Author: Tony Ho
* Purpose: 註冊畫面的ViewModel
* Example: none
* Variable: none
* Output: none
* Return Code: none
*
*****************************************************************/
using System.ComponentModel.DataAnnotations;


namespace DL.Web.ViewModels.Account
{
    public class RegisterViewModel
    {

        [Required]
        [Display(Name = "員工編號")]
        [StringLength(4)]
        [System.Web.Mvc.Remote("CheckUserName", "Validate", ErrorMessage = "此帳號已申請過")]
        public string UserAccount { get; set; }

        [Required]
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }

        [Required]
        [Display(Name ="確認密碼")]
        [DataType(DataType.Password)]
        [Compare("UserPassword", ErrorMessage = "密碼和確認密碼不相符。")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "姓名")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "電子郵件")]
        public string UserEmail { get; set; }
    }
}