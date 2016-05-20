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
using DL.Web.Utilities.Attribute;
using System.ComponentModel.DataAnnotations;


namespace DL.Web.ViewModels.Account
{
    public class RegisterVM
    {

        [Required]
        [Display(Name = "員工編號")]
        [StringLength(4,ErrorMessage = "至少輸入4個字元",MinimumLength =4)]
        [RegularExpression("^\\d{4,}$",ErrorMessage ="輸入的格式不正確，正確的格式為4個數字")]
        [System.Web.Mvc.Remote("CheckUserName", "Validate", ErrorMessage = "員工編號已申請過")]
        public string UserAccount { get; set; }

        [Required]
        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        [StringLength(18, ErrorMessage = "至少輸入4個字元", MinimumLength = 4)]
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
        [Email(ErrorMessage="請輸入正確格式。XXX@86shop.com.tw")]
        public string UserEmail { get; set; }
    }
}