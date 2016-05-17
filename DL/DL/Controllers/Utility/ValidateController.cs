using DL.Models;
using DL.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DL.Web.Controllers
{
    public class ValidateController : Controller
    {
        GenericRepository<User> _genericRepository = null;


        public ValidateController()
        {
            this._genericRepository = new GenericRepository<User>(new DiaryLogDBEntities());

        }

        public ActionResult CheckUserName(string UserAccount)
        {
            bool isValidate = true;
            //if (Url.IsLocalUrl(Request.Url.AbsoluteUri))
            //{

            User user = _genericRepository.GetAll().Where(x => x.UserAccount == UserAccount).FirstOrDefault();

            if (user != null)
            {
                isValidate = false;
            }

            //}
            // Remote 驗證是使用 Get 因此要開放
            return Json(isValidate, JsonRequestBehavior.AllowGet);
        }
    }
}