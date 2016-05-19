using DL.Models;
using DL.Models.Repository;
using DL.Models.Service.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DL.Web.Controllers
{
    public class ValidateController : Controller
    {

        public ActionResult CheckUserName(string UserAccount)
        {
            UserService _userService = new UserService();

            bool isValidate = true;

            User user = _userService.FindUsersByAccount(UserAccount).FirstOrDefault();

            if (user != null)
            {
                isValidate = false;
            }

            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }
    }
}