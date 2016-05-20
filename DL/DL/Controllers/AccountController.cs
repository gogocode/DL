/*****************************************************************
* Date: 2016/05/07
* Author: Tony Ho
* Purpose: 實作登入畫面CRUD
* Example: none
* Variable: none
* Output: none
* Return Code: none
*
*****************************************************************/
using AutoMapper;
using DL.Models;
using DL.Models.Repository;
using DL.Models.Service;
using DL.Models.Service.Users;
using DL.Web.ActionFilter;
using DL.Web.Controllers.Base;
using DL.Models.Repository.Class.Base;
using DL.Web.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DL.Models.Repository.Class;
using DL.Web.Utilities;

namespace DL.Web.Controllers
{
    public class AccountController : BaseController
    {

        UserService _userService = null;

        #region Login&Logout

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 登入時清空所有 Session 資料
            Session.RemoveAll();
            //MD5加密密碼
            string encrypt = MD5Encoder.Encrypt(model.Password);
            UserService _userService = new UserService();

            ValidateLoginSM validateLoginSV = _userService.ValidateLogin(model.AccountId, encrypt);
            int loginStatus = validateLoginSV.LoginStatus;

            if (loginStatus == 0)
            {
                ModelState.AddModelError(string.Empty, "登入失敗，請重新登入。");
                ModelState.AddModelError(string.Empty, "員工編號或密碼輸入錯誤。");

                return View(model);
            }
            else if (loginStatus == 1)
            {
                ModelState.AddModelError(string.Empty, "登入失敗，請重新登入。");
                ModelState.AddModelError(string.Empty, "帳號未啟動，請聯絡管理員。");

                return View(model);
            }
            else
            {
                Session["Id"] = validateLoginSV.UserId;
                Session["Account"] = validateLoginSV.UserAccount;

                if (Session["Account"].ToString().Equals("9999"))
                {
                    //return RedirectToAction("Index","User");
                    return RedirectToAction("MasterIndex", "DiaryLogNew");
                }

                return RedirectToAction("Index", "DiaryLogNew",new {userId = validateLoginSV.UserId });
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.RemoveAll();

            return RedirectToAction("Login");
        }

        #endregion

        #region Register

        [HttpGet]
        public ActionResult Register()
        {
            RegisterVM registerVM = new RegisterVM();
            registerVM.UserEmail = "@86shop.com.tw";

            return View(registerVM);
        }

        [HttpPost]
        public ActionResult Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            _userService = new UserService();

            DateTime nowDate = System.DateTime.Now;

            Mapper.CreateMap<RegisterVM, User>();
            User user = Mapper.Map<User>(registerVM);
            user.UserPassword = MD5Encoder.Encrypt(registerVM.UserPassword);
            user.UserStatus = "0";//0帳號未啟動 1帳號啟動
            user.CreateDate = nowDate;
            user.CreateId = "9999";
            user.UpdateDate = nowDate;
            user.UpdateId = "9999";

            using (UserRepository _repo = new UserRepository())
            { _repo.Insert(user); }
                

            return RedirectToAction("RegistSuccess");
        }

        [HttpGet]
        public ActionResult RegistSuccess()
        {
            return View();
        }

        #endregion
    }
}