using AutoMapper;
using DL.Models;
using DL.Models.Service.ServiceModels.User;
using DL.Models.Service.Users;
using DL.Web.ActionFilter;
using DL.Web.Controllers.Base;
using DL.Web.Utilities;
using DL.Web.ViewModels.User;
using MvcPaging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DL.Web.Controllers
{
    public class UserController : BaseController
    {

        private int PageSize = Web.Properties.Settings.Default.PageSize;


        #region 查詢

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Index()
        {
            UserService _userService = new UserService();
            UserIndexVM userIdexVM = new UserIndexVM();
            List<User> users = _userService.FindAllUsers();

            userIdexVM.Users = users.OrderBy(p => p.UserAccount).ToPagedList(userIdexVM.Page > 0 ? userIdexVM.Page - 1 : 0, PageSize);

            return View(userIdexVM);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult Index(UserIndexVM userIdexVM)
        {
            UserService _userService = new UserService();

            List<User> users = _userService.FindUsers(userIdexVM.UserAccount,userIdexVM.UserName);

            userIdexVM.Users = users.OrderBy(p => p.UserAccount).ToPagedList(userIdexVM.Page > 0 ? userIdexVM.Page - 1 : 0, PageSize);

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

            UserService _userService = new UserService();

            User user = _userService.GetUserById(id.Value);

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
            UserService _userService = new UserService();
            _userService.DeleteUserById(id);

            return RedirectToAction("Index");
        }

        #endregion

        #region 啟動&停用

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult StartUp(int id)
        {
            UserService _userService = new UserService();
            _userService.StartUp(id);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult EndUp(int id)
        {
            UserService _userService = new UserService();
            _userService.EndUp(id);

            return RedirectToAction("Index");
        }

        #endregion

    }
}