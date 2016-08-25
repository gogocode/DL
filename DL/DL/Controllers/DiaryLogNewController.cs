﻿using AutoMapper;
using DL.Models;
using DL.Models.Repository;
using DL.Models.Service;
using DL.Models.Service.DiaryLogs;
using DL.Models.Service.ServiceModels;
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
        private UserService _userService = new UserService();
        private DiaryLogService _diaryLogService = new DiaryLogService();

        #region Index

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Index(int userId)
        {
            //檢查如果userId不等於Session["Id"]，不能進入頁面看別人的。
            //除了管理者除外
            if(userId.ToString() != Session["Id"].ToString())
            {
                if (!Session["Account"].ToString().Equals("9999"))
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            DiaryLogService _diaryLogService = new DiaryLogService();
            List<DateTime> diaryLogDates = _diaryLogService.GetDiarysGroupByUserId(userId);
            ViewBag.Account = Session["Account"].ToString(); 

            DiaryLogNewIndexVM diaryLogNewIndexVM = new DiaryLogNewIndexVM();
            diaryLogNewIndexVM.DiaryLogDate = diaryLogDates.OrderByDescending(x => x.Date).ToPagedList(diaryLogNewIndexVM.Page > 0 ? diaryLogNewIndexVM.Page - 1 : 0, PageSize);
            diaryLogNewIndexVM.UserId = userId;

            return View(diaryLogNewIndexVM);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult Index(DiaryLogNewIndexVM model)
        {
            //檢查如果userId不等於Session["Id"]，不能進入頁面看別人的。
            //除了管理者除外
            if (model.UserId.ToString() != Session["Id"].ToString())
            {
                if (!Session["Account"].ToString().Equals("9999"))
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            List<DateTime> diaryLogDates = _diaryLogService.GetDiarysGroupByUserId(model.UserId,model.dateStart,model.dateEnd);
            ViewBag.Account = Session["Account"].ToString();

            DiaryLogNewIndexVM diaryLogNewIndexVM = model;
            diaryLogNewIndexVM.DiaryLogDate = diaryLogDates.OrderByDescending(x => x.Date).ToPagedList(diaryLogNewIndexVM.Page > 0 ? diaryLogNewIndexVM.Page - 1 : 0, PageSize);

            return View("Index",diaryLogNewIndexVM);
        }

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult MasterIndex()
        {
            //只有管理者才能進入頁面
            if (!Session["Account"].ToString().Equals("9999"))
            {
                return RedirectToAction("Login", "Account");
            }

            UserAndDiaryLogService _service = new UserAndDiaryLogService();
            List<DiaryLogAndUserSM> diaryLogAndUserSMs = _service.FindAllUserJoinDiaryLog();

            List<UserGroupVM> userGroupVM = diaryLogAndUserSMs.GroupBy(item =>new {item.diaryLog.UserId, item.UserAccount ,item.UserName})
            .Select(group => new UserGroupVM {UserId =group.Key.UserId, UserAccount = group.Key.UserAccount, UserName = group.Key.UserName  })
            .ToList();

            DiaryLogNewMasterIndexVM diaryLogNewMasterIndexVM = new DiaryLogNewMasterIndexVM();
            diaryLogNewMasterIndexVM.UserGroups = userGroupVM.OrderBy(x=>x.UserAccount).ToPagedList(diaryLogNewMasterIndexVM.Page > 0 ? diaryLogNewMasterIndexVM.Page - 1 : 0, PageSize);

            return View(diaryLogNewMasterIndexVM);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult MasterIndex(DiaryLogNewMasterIndexVM model)
        {
            //只有管理者才能進入頁面
            if (!Session["Account"].ToString().Equals("9999"))
            {
                return RedirectToAction("Login", "Account");
            }

            UserAndDiaryLogService _service = new UserAndDiaryLogService();
            List<DiaryLogAndUserSM> diaryLogAndUserSMs = _service.FindUserJoinDiaryLogByAccountId(model.UserAccount,model.UserName);

            List<UserGroupVM> userGroupVM = diaryLogAndUserSMs.GroupBy(item => new { item.diaryLog.UserId, item.UserAccount, item.UserName })
            .Select(group => new UserGroupVM { UserId = group.Key.UserId, UserAccount = group.Key.UserAccount, UserName = group.Key.UserName })
            .ToList();

            DiaryLogNewMasterIndexVM diaryLogNewMasterIndexVM = model;
            diaryLogNewMasterIndexVM.UserGroups = userGroupVM.OrderBy(x => x.UserAccount).ToPagedList(diaryLogNewMasterIndexVM.Page > 0 ? diaryLogNewMasterIndexVM.Page - 1 : 0, PageSize);

            return View(diaryLogNewMasterIndexVM);
        }

        #endregion

        #region Edit

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Edit(string strDate)
        {
            DiaryLogService _diaryLogService = new DiaryLogService();
            string userAccount = Session["Account"].ToString();
            int userId = Convert.ToInt32(Session["Id"].ToString());
            string userName = _userService.GetUserNameById(userId);

            ViewBag.DiaryLogItems = GetDiaryLogItemSelectItems(userId);

            List<DiaryLog> diaryLogs = _diaryLogService.GetDiaryLogsByDate(strDate,userId);
            Mapper.CreateMap<DiaryLog, DiaryLogDetailVM>();
            List<DiaryLogDetailVM> diaryLogsVM = Mapper.Map<List<DiaryLogDetailVM>>(diaryLogs);

            DiaryLogNewEditVM diaryLogNewEidts = new DiaryLogNewEditVM();
            diaryLogNewEidts.UserAccount = userAccount;
            diaryLogNewEidts.UserName = userName;
            diaryLogNewEidts.DiaryLogDate = Convert.ToDateTime(strDate);
            diaryLogNewEidts.DiaryLogs = diaryLogsVM;
            diaryLogNewEidts.UserId = userId;

            return View(diaryLogNewEidts);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult AjaxEdit(DiaryLogNewEditVM model)
        {

            if (model.DiaryLogs == null || model.DiaryLogDate == null)
            {
                return Json("false,");
            }

            //移除不需要的驗證
            foreach (var item in ModelState.Keys)
            {
                if (item.Contains("DiaryLogItems"))
                {
                    // ModelState.Remove(item);
                    ModelState[item].Errors.Clear();
                }
            }

            if (!ModelState.IsValid)
            {
                return Json("false,");
            }

            int userId = Convert.ToInt32(Session["Id"].ToString());
            string account = Session["Account"].ToString();
            DiaryLogService _diaryLogService = new DiaryLogService();
            DateTime diaryLogDate = model.DiaryLogDate;

            Mapper.CreateMap<DiaryLogDetailVM, DiaryLog>();
            List<DiaryLog> diaryLogs = Mapper.Map<List<DiaryLog>>(model.DiaryLogs);
            _diaryLogService.ModidDiaryLogy(diaryLogs, diaryLogDate, account, userId);


            return Json("true," + userId.ToString());
        }

        #endregion

        #region Create

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Create()
        {
            string userAccount = Session["Account"].ToString();
            int userId = Convert.ToInt32(Session["Id"].ToString());
            string userName = _userService.GetUserNameById(userId);

            DiaryLogNewCreateVM diaryLogNewEidts = new DiaryLogNewCreateVM()
            {
                UserAccount = userAccount,
                UserName = userName,
                DiaryLogDate = DateTime.Now.Date,
                UserId = userId
            };

            return View(diaryLogNewEidts);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult AjaxCreate(DiaryLogNewCreateVM model)
        {

            if (model.DiaryLogs == null || model.DiaryLogDate == null)
            {
                return Json("false,");
            }

            //移除不需要的驗證
            foreach(var item in ModelState.Keys)
            {
                if( item.Contains("DiaryLogItems"))
                { 
                    // ModelState.Remove(item);
                    ModelState[item].Errors.Clear();
                }
            }

            if (!ModelState.IsValid)
            {
                return Json("false,");
            }

            int userId = Convert.ToInt32(Session["Id"].ToString());
            string account = Session["Account"].ToString();
            DiaryLogService _diaryLogService = new DiaryLogService();

            foreach (var item in model.DiaryLogs)
            {
                DiaryLog diaryLog = new DiaryLog();
                diaryLog.DiaryLogItem = item.DiaryLogItem;
                diaryLog.DiaryLogContents = item.DiaryLogContents;
                diaryLog.DiaryLogStatus = item.DiaryLogStatus;
                diaryLog.DiaryLogHours = item.DiaryLogHours;
                diaryLog.DiaryLogSituation = item.DiaryLogSituation;
                diaryLog.DiaryLogSolve = item.DiaryLogSolve;

                _diaryLogService.InsertDiaryLog(diaryLog, model.DiaryLogDate, account, userId);
            }

            return Json("true," + userId.ToString());
        }

        #endregion

        #region Delete

        public ActionResult DeleteADetail(int actdetailNo)
        {
            DiaryLogService _diaryLogService = new DiaryLogService();

            return Json(_diaryLogService.DeleteDiaryLog(actdetailNo));
        }

        #endregion

        #region Detail

        public ActionResult Detail(string strDate,int userId)
        {
            //檢查如果userId不等於Session["Id"]，不能進入頁面看別人的。
            //除了管理者除外
            if (userId.ToString() != Session["Id"].ToString())
            {
                if (!Session["Account"].ToString().Equals("9999"))
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            string userAccount = _userService.GetUserAccountById(userId);
            string userName = _userService.GetUserNameById(userId);

            List<DiaryLog> diaryLogs = _diaryLogService.GetDiaryLogsByDate(strDate, userId);

            Mapper.CreateMap<DiaryLog, DiaryLogDetailVM>();
            List<DiaryLogDetailVM> diaryLogsVM = Mapper.Map<List<DiaryLogDetailVM>>(diaryLogs);

            DiaryLogNewEditVM diaryLogNewEidts = new DiaryLogNewEditVM();
            diaryLogNewEidts.UserId = userId;
            diaryLogNewEidts.UserAccount = userAccount;
            diaryLogNewEidts.UserName = userName;
            diaryLogNewEidts.DiaryLogDate = Convert.ToDateTime(strDate);
            diaryLogNewEidts.DiaryLogs = diaryLogsVM;

            return View(diaryLogNewEidts);
        }

        #endregion

        public ActionResult GetDetailPartialView(int userId)
        {
            ViewBag.DiaryLogItems = GetDiaryLogItemSelectItems(userId);

            return PartialView("_DiaryLogDetailPartialView");
        }

        private List<SelectListItem> GetDiaryLogItemSelectItems(int userId)
        {
            Dictionary<string, string> diaryLogItemDics = _diaryLogService.GetDiaryLogItemsByUserId(userId);
            List<SelectListItem> diaryLogItems = new List<SelectListItem>();

            diaryLogItems.Add(new SelectListItem { Value = "", Text = "請選擇" });
            foreach (var diaryLogItemDic in diaryLogItemDics)
            {
                diaryLogItems.Add(new SelectListItem { Value = diaryLogItemDic.Key, Text = diaryLogItemDic.Value });
            }

            return diaryLogItems;
        }
    }
}