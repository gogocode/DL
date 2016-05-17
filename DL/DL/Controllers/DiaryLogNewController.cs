using DL.Models;
using DL.Models.Repository;
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
        GenericRepository<DiaryLog> _genericRepository = null;
        GenericRepository<User> _userRepository = null;
        private int PageSize = Web.Properties.Settings.Default.PageSize;

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var grouping = Idioms.GroupBy(ui => ui.Idiom);

        //    var duplicates = grouping.Where(group => group.Count() > 1);

        //    if (duplicates.Any())
        //    {
        //        string message = string.Empty;

        //        foreach (var duplicate in duplicates)
        //        {
        //            message += string.Format("{0} was selected {1} times", duplicate.Key, duplicate.Count());
        //        }

        //        yield return new ValidationResult(message, new[] { "Idioms" });
        //    }
        //}

        public DiaryLogNewController()
        {
            this._genericRepository = new GenericRepository<DiaryLog>(new DiaryLogDBEntities());
            this._userRepository = new GenericRepository<User>(new DiaryLogDBEntities());
        }

        #region Search

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Index()
        {

            IQueryable<DiaryLog> diaryLogs = _genericRepository.GetAll();
            int userId = Convert.ToInt32(Session["Id"].ToString());

            if (!Session["Account"].ToString().Equals("9999"))
            {
                diaryLogs = diaryLogs.Where(x => x.UserId.Equals(userId));
            }

            var gro = from d in diaryLogs
                      group d by d.DiaryLogDate;
            List<DateTime> diaryLogDates = new List<DateTime>();
            foreach (var item in gro)
            {
                DateTime diaryLogDate = item.Key;
                diaryLogDates.Add(diaryLogDate);
            }


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

            DateTime date = Convert.ToDateTime(strDate);

            int userAccount = Convert.ToInt32(Session["Account"].ToString());
            int userId = Convert.ToInt32(Session["Id"].ToString());
            string userName = _userRepository.GetAll().Where(x => x.UserId == userId).FirstOrDefault().UserName;
            List<DiaryLog> diaryLogs = _genericRepository.GetAll().Where(x => x.UserId == userId && x.DiaryLogDate == date).ToList();
            DiaryLogNewEditViewModel diaryLogNewEidts = new DiaryLogNewEditViewModel();
            diaryLogNewEidts.UserAccount = userAccount;
            diaryLogNewEidts.UserName = userName;
            diaryLogNewEidts.DiaryLogDate = date;
            diaryLogNewEidts.DiaryLogs = diaryLogs;

            return View(diaryLogNewEidts);
        }

        [HttpPost]
        [CheckSessionAcitionFilter]
        public ActionResult Edit(DiaryLogNewEditViewModel diaryLogNewEidts)
        {

            DateTime diaryLogDate = diaryLogNewEidts.DiaryLogDate;

            foreach (var item in diaryLogNewEidts.DiaryLogs)
            {
                DiaryLog diaryLog = _genericRepository.GetById(item.DiaryLogId);

                if (diaryLog != null)
                {
                    diaryLog.DiaryLogItem = item.DiaryLogItem;
                    diaryLog.DiaryLogContents = item.DiaryLogContents;
                    diaryLog.DiaryLogStatus = item.DiaryLogStatus;
                    diaryLog.DiaryLogHours = item.DiaryLogHours;
                    diaryLog.DiaryLogSituation = item.DiaryLogSituation;
                    diaryLog.DiaryLogSolve = item.DiaryLogSolve;
                    diaryLog.UpdateDate = DateTime.Now;
                    diaryLog.UpdateId = Session["Account"].ToString();

                    _genericRepository.Edit(diaryLog);
                }
                else
                {
                    DiaryLog newDiaryLog = new DiaryLog();

                    newDiaryLog.DiaryLogDate = diaryLogDate;
                    newDiaryLog.UserId = Convert.ToInt32(Session["Id"].ToString());
                    newDiaryLog.DiaryLogItem = item.DiaryLogItem;
                    newDiaryLog.DiaryLogContents = item.DiaryLogContents;
                    newDiaryLog.DiaryLogStatus = item.DiaryLogStatus;
                    newDiaryLog.DiaryLogHours = item.DiaryLogHours;
                    newDiaryLog.DiaryLogSituation = item.DiaryLogSituation;
                    newDiaryLog.DiaryLogSolve = item.DiaryLogSolve;
                    newDiaryLog.CreateDate = DateTime.Now;
                    newDiaryLog.CreateId = Session["Account"].ToString();
                    newDiaryLog.UpdateDate = DateTime.Now;
                    newDiaryLog.UpdateId = Session["Account"].ToString();

                    _genericRepository.Insert(newDiaryLog);
                }
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Create

        [HttpGet]
        [CheckSessionAcitionFilter]
        public ActionResult Create()
        {

            int userAccount = Convert.ToInt32(Session["Account"].ToString());
            int userId = Convert.ToInt32(Session["Id"].ToString());
            string userName = _userRepository.GetAll().Where(x => x.UserId == userId).FirstOrDefault().UserName;
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

            string Id = Session["Id"].ToString();
            string Account = Session["Account"].ToString();

            foreach (var item in model.DiaryLogs)
            {
                DiaryLog diaryLog = new DiaryLog();

                diaryLog.UserId = Convert.ToInt32(Id);
                diaryLog.DiaryLogDate = model.DiaryLogDate;
                diaryLog.DiaryLogItem = item.DiaryLogItem;
                diaryLog.DiaryLogContents = item.DiaryLogContents;
                diaryLog.DiaryLogStatus = item.DiaryLogStatus;
                diaryLog.DiaryLogHours = item.DiaryLogHours;
                diaryLog.DiaryLogSituation = item.DiaryLogSituation;
                diaryLog.DiaryLogSolve = item.DiaryLogSolve;
                diaryLog.CreateDate = DateTime.Now;
                diaryLog.CreateId = Account;
                diaryLog.UpdateDate = DateTime.Now;
                diaryLog.UpdateId = Account;

                _genericRepository.Insert(diaryLog);
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
            bool result = false;

            var detail = _genericRepository.GetById(actdetailNo);
            if (detail != null)
            {
                _genericRepository.Delete(detail);
                result = true;
            }

            return Json(result);
        }


    }
}