using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DL.Web.ViewModels.Test
{
    public class TestValidateFrontEnd
    {
        [Required]
        [Display(Name ="姓名")]
        public string Name { get; set; }

        [Required]
        [Display(Name ="地址")]
        public string Address { get; set; }
    }
}