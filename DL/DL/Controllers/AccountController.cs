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
using DL.Web.ActionFilter;
using DL.Web.Controllers.Base;
using DL.Web.Services;
using DL.Web.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DL.Web.Controllers
{
    public class AccountController : BaseController
    {
        GenericRepository<User> _genericRepository = null;
        AccountService _service = null;

        public AccountController()
        {
            this._genericRepository = new GenericRepository<User>(new DiaryLogDBEntities());
            this._service = new AccountService(); 
        }

        #region Login&Logout

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 登入時清空所有 Session 資料
            Session.RemoveAll();

            string strPassword = model.Password;
            string strUsername = model.AccountId;
            int loginStatus = ValidateLogin(strUsername, strPassword);

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
                if (Session["Account"].ToString().Equals("9999"))
                {
                    return RedirectToAction("Index","User");
                }

                //return RedirectToAction("Index", "DiaryLog");

                return RedirectToAction("Index", "DiaryLogNew");
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
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            if(_service.IsAccountExist(registerVM.UserAccount))
            {
                ModelState.AddModelError(string.Empty, "員工編號已存在。");

                return View(registerVM);
            }

            DateTime nowDate = System.DateTime.Now;

            Mapper.CreateMap<RegisterViewModel, User>();
            User user = Mapper.Map<User>(registerVM);
            user.UserStatus = "0";//0帳號未啟動 1帳號啟動
            user.CreateDate = nowDate;
            user.CreateId = "9999";
            user.UpdateDate = nowDate;
            user.UpdateId = "9999";

            _genericRepository.Insert(user);

            return RedirectToAction("RegistSuccess");
        }

        [HttpGet]
        public ActionResult RegistSuccess()
        {
            return View();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// 驗證使用者是否登入成功
        /// </summary>
        /// <param name="strUsername">登入帳號</param>
        /// <param name="strPassword">登入密碼</param>
        /// <returns>loginStatus為0 帳號或密碼輸入錯誤 loginStatus為1 帳號未啟動 loginStatus為2 登入正常</returns>
        private int ValidateLogin(string strUsername, string strPassword)
        {

            //驗證
            //檢查 Username, Password 是否正確
            User user = _genericRepository.GetAll().Where(x => x.UserAccount.Equals(strUsername) && x.UserPassword.Equals(strPassword)).FirstOrDefault();

            if (user != null)
            {
                if (user.UserStatus == "0" || string.IsNullOrEmpty(user.UserStatus))
                {
                    return 1;
                }

                Session["Id"] = user.UserId;
                Session["Account"] = user.UserAccount;

                return 2;
            }

            return 0;
        }
        #endregion

        #region Ajax

        [HttpPost]
        public JsonResult CheckAccount(string account)
        {
            User user = _genericRepository.GetAll().Where(x => x.UserAccount == account).FirstOrDefault();

            if(user != null)
            {
                return Json("true");
            }

            return Json("false");
        }

        #endregion
    }
}