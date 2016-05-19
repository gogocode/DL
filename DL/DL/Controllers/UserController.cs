using AutoMapper;
using DL.Models;
using DL.Models.Repository;
using DL.Models.Service.ServiceModels.User;
using DL.Models.Service.Users;
using DL.Web.ActionFilter;
using DL.Web.Utilities;
using DL.Web.ViewModels.User;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DL.Web.Controllers
{
    public class UserController : Controller
    {
        GenericRepository<User> _genericRepository = null;
        private int PageSize = Web.Properties.Settings.Default.PageSize;

        public UserController()
        {
            this._genericRepository = new GenericRepository<User>(new DiaryLogDBEntities());
        }

        #region 查詢

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Index()
        {
            UserService _userService = new UserService();
            UserIndexVM userIdexVM = new UserIndexVM();
            List<User> users = _userService.FindAllUsers().ToList();

            userIdexVM.Users = users.OrderBy(p => p.UserStatus).ToPagedList(userIdexVM.Page > 0 ? userIdexVM.Page - 1 : 0, PageSize);

            return View(userIdexVM);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult Index(UserIndexVM userIdexVM)
        {
            UserService _userService = new UserService();

            List<User> users = _userService.FindUsersByAccount(userIdexVM.UserAccount);

            userIdexVM.Users = users.OrderBy(p => p.UserStatus).ToPagedList(userIdexVM.Page > 0 ? userIdexVM.Page - 1 : 0, PageSize);

            return View(userIdexVM);
        }

        #endregion

        #region 編輯

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserService _userService = new UserService();
            User user = _userService.GetUserById(id.Value);

            if (user == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<User, UserEditVM>();
            UserEditVM userVM = Mapper.Map<UserEditVM>(user);

            //SupperUser不可更改員工密碼
            if (Session["Account"].ToString() == "9999")
            {
                TempData["IsSupperUser"] = true;
            }  

            userVM.ConfirmPassword = user.UserPassword;
            userVM.OriginPassword = user.UserPassword;

            return View(userVM);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditVM userVM)
        {
            UserService _userService = new UserService();

            if (!ModelState.IsValid)
            {
                return View(userVM);
            }

            //MD5無法解密，故用此判斷是否更改過密碼
            //無更改過密碼，已經為MD5密碼
            if (userVM.UserPassword.Equals(userVM.OriginPassword))
            {
            }
            else//更改過密碼，需加密為MD5
            {
                userVM.UserPassword = MD5Encoder.Encrypt(userVM.UserPassword);
            }

            Mapper.CreateMap<UserEditVM, UserEditSV>();
            UserEditSV userEditSV = Mapper.Map<UserEditSV>(userVM);
            userEditSV.UpdateDate = System.DateTime.Now;
            userEditSV.UpdateId = Session["Account"].ToString();

            _userService.ModifyUser(userEditSV);

            if(Session["Account"].Equals("9999"))
            { 
            return RedirectToAction("Index");
            }

            return RedirectToAction("EditSuccess");
        }

        [HttpGet]
        public ActionResult EditSuccess()
        {
            return View();
        }

        #endregion

        #region 刪除
        [CheckSessionAcitionFilter]
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _genericRepository.GetById(id.Value);

            if (user == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<User, UserDeleteVM>();
            UserDeleteVM userDeleteVM = Mapper.Map<UserDeleteVM>(user);

            return View(userDeleteVM);
        }

        // POST: DiaryLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CheckSessionAcitionFilter]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = _genericRepository.GetById(id);
            _genericRepository.Delete(user);

            return RedirectToAction("Index");
        }

        #endregion

        #region 啟動

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult StartUp(int id)
        {
            User user = _genericRepository.GetById(id);
            user.UserStatus = "1";
            _genericRepository.Edit(user);

            return RedirectToAction("Index");
        }

        #endregion

        #region Private Methods

        #region Private Methods

        private bool IsSessionExist()
        {
            if (Session["Id"] == null || Session["Account"] == null)
            {
                return false;
            }

            return true;
        }

        #endregion

        #endregion
    }
}