using AutoMapper;
using DL.Models;
using DL.Models.Repository;
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
        public ActionResult Index()
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            UserIndexViewModel userIdexVM = new UserIndexViewModel();

            return View(userIdexVM);
        }

        [HttpPost]
        public ActionResult Index(UserIndexViewModel userIdexVM)
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            IQueryable<User> users = _genericRepository.GetAll();

            if (!string.IsNullOrEmpty(userIdexVM.UserAccount))
            {
                users = users.Where(x => x.UserAccount.Contains(userIdexVM.UserAccount));
            }

            userIdexVM.Users = users.OrderBy(p => p.UserStatus).ToPagedList(userIdexVM.Page > 0 ? userIdexVM.Page - 1 : 0, PageSize);

            return View(userIdexVM);
        }

        #endregion

        #region 編輯

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            User user = _genericRepository.GetById(id.Value);

            if (user == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<User, UserEditViewModel>();
            UserEditViewModel userVM = Mapper.Map<UserEditViewModel>(user);
            userVM.ConfirmPassword = user.UserPassword;

            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel userVM)
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            User user = _genericRepository.GetById(userVM.UserId);

            if (ModelState.IsValid)
            {
                user.UserName = userVM.UserName;
                user.UserPassword = userVM.UserPassword;
                user.UserEmail = userVM.UserEmail;
                user.UpdateDate = System.DateTime.Now;
                user.UpdateId = Session["Account"].ToString();

                _genericRepository.Edit(user);

                return RedirectToAction("Index");
            }

            return View(userVM);
        }

        #endregion

        #region 刪除

        public ActionResult Delete(int? id)
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = _genericRepository.GetById(id.Value);

            if (user == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<User, UserDeleteViewModel>();
            UserDeleteViewModel userDeleteVM = Mapper.Map<UserDeleteViewModel>(user);

            return View(userDeleteVM);
        }

        // POST: DiaryLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = _genericRepository.GetById(id);
            _genericRepository.Delete(user);

            return RedirectToAction("Index");
        }

        #endregion

        [HttpGet]
        public ActionResult StartUp(int id)
        {
            User user = _genericRepository.GetById(id);
            user.UserStatus = "1";
            _genericRepository.Edit(user);

            return RedirectToAction("Index");
        }

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