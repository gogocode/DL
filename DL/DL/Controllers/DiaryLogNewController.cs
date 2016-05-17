using DL.Models;
using DL.Models.Repository;
using DL.Models.Service.DiaryLogs;
using DL.Models.Service.Users;
using DL.Web.ActionFilter;
using DL.Web.Controllers.Base;
using DL.Web.ViewModels.DiaryLogNew;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DL.Web.Controllers
{
    public class DiaryLogNewController : BaseController
    {

        private int PageSize = Web.Properties.Settings.Default.PageSize;


        #region Search

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Index()
        {
            int userId = Convert.ToInt32(Session["Id"].ToString());
            string account = Session["Account"].ToString();

            DiaryLogService _diaryLogService = new DiaryLogService();
            List<DateTime> diaryLogDates = _diaryLogService.GetDiarysGroupByUserId(account, userId);

            DiaryLogNewIndexViewModel diaryLogNewIndexVM = new DiaryLogNewIndexViewModel();
            diaryLogNewIndexVM.DiaryLogDate = diaryLogDates.OrderByDescending(x => x.Date).ToPagedList(diaryLogNewIndexVM.Page > 0 ? diaryLogNewIndexVM.Page - 1 : 0, PageSize);

            return View(diaryLogNewIndexVM);
        }

        #endregion

        #region Edit

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Edit(string strDate)
        {
            UserService _userService = new UserService();
            DiaryLogService _diaryLogService = new DiaryLogService();

            

            string userAccount = Session["Account"].ToString();
            int userId = Convert.ToInt32(Session["Id"].ToString());

            string userName = _userService.GetUserNameById(userId);


            List<DiaryLog> diaryLogs = _diaryLogService.GetDiaryLogsByDate(strDate,userId);

            DiaryLogNewEditViewModel diaryLogNewEidts = new DiaryLogNewEditViewModel();
            diaryLogNewEidts.UserAccount = userAccount;
            diaryLogNewEidts.UserName = userName;
            diaryLogNewEidts.DiaryLogDate = Convert.ToDateTime(strDate);
            diaryLogNewEidts.DiaryLogs = diaryLogs;

            return View(diaryLogNewEidts);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult Edit(DiaryLogNewEditViewModel diaryLogNewEidts)
        {

            DiaryLogService _diaryLogService = new DiaryLogService();
            DateTime diaryLogDate = diaryLogNewEidts.DiaryLogDate;
            string account = Session["Account"].ToString();
            int userId = Convert.ToInt32( Session["Id"].ToString());

            _diaryLogService.ModidDiaryLogy(diaryLogNewEidts.DiaryLogs.ToList(), diaryLogDate, account, userId);

            return RedirectToAction("Index");
        }

        #endregion

        #region Create

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Create()
        {
            UserService _userService = new UserService();
            string userAccount = Session["Account"].ToString();
            int userId = Convert.ToInt32(Session["Id"].ToString());

            string userName = _userService.GetUserNameById(userId);


            DiaryLogNewEditViewModel diaryLogNewEidts = new DiaryLogNewEditViewModel();
            diaryLogNewEidts.UserAccount = userAccount;
            diaryLogNewEidts.UserName = userName;
            diaryLogNewEidts.DiaryLogDate = DateTime.Now.Date;

            return View(diaryLogNewEidts);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult Create(DiaryLogNewCreateViewModel model)
        {

            if (model.DiaryLogs == null || model.DiaryLogDate == null)
            {
                return View(model);
            }

            int id = Convert.ToInt32( Session["Id"].ToString());
            string account = Session["Account"].ToString();
            DiaryLogService _diaryLogService = new DiaryLogService();


            foreach (var item in model.DiaryLogs)
            {
                _diaryLogService.InsertDiaryLog(item, model.DiaryLogDate, account, id);
            }

            return RedirectToAction("Index");
        }

        #endregion

        public ActionResult GetDetailPartialView()
        {
            return PartialView("_DiaryLogDetailPartialView");
        }

        public ActionResult DeleteADetail(int actdetailNo)
        {
            DiaryLogService _diaryLogService = new DiaryLogService();

            return Json(_diaryLogService.DeleteDiaryLog(actdetailNo));
        }


    }
}