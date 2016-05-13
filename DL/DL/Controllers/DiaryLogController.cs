/*****************************************************************
* Date: 2016/05/07
* Author: Tony Ho
* Purpose: 實作工作日誌CRUD
* Example: none
* Variable: none
* Output: none
* Return Code: none
*
*****************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DL.Models;
using DL.ViewModels;
using DL.ViewModels.DiaryLog;
using MvcPaging;
using DL.Models.Repository;
using AutoMapper;
using DL.Web.ViewModels.DiaryLog;

namespace DL.Web.Controllers
{
    public class DiaryLogController : Controller
    {
        GenericRepository<DiaryLog> _genericRepository = null;
        private int PageSize = Web.Properties.Settings.Default.PageSize;

        public DiaryLogController()
        {
           this._genericRepository = new GenericRepository<DiaryLog>(new DiaryLogDBEntities());
        }

        #region 查詢
        [HttpGet]
        public ActionResult Index()
        {

            if(!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            DiaryLogSearchViewModel diaryLogSearch = new DiaryLogSearchViewModel();

            return View(diaryLogSearch);
        }

        

        [HttpPost]
        public ActionResult Index(DiaryLogSearchViewModel diaryLogSearch)
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            IQueryable<DiaryLog> diaryLogs = _genericRepository.GetAll();

            int userId = Convert.ToInt32(Session["Id"].ToString());

            if (!Session["Account"].ToString().Equals("9999"))
            {
                diaryLogs = diaryLogs.Where(x => x.UserId.Equals(userId));
            }

            if (diaryLogSearch.dateStart != null)
            {
                diaryLogs = diaryLogs.Where(x=>x.DiaryLogDate >= diaryLogSearch.dateStart);
            }

            if (diaryLogSearch.dateEnd != null)
            {
                diaryLogs = diaryLogs.Where(x => x.DiaryLogDate <= diaryLogSearch.dateEnd);
            }

            if (!string.IsNullOrEmpty(diaryLogSearch.content))
            {
                diaryLogs = diaryLogs.Where(x => x.DiaryLogContents.Contains(diaryLogSearch.content));
            }

            var gro = from d in diaryLogs
                      group d by d.DiaryLogDate;

            List<DiaryLogGroupByDateViewModel> diaryLogGroups = new List<DiaryLogGroupByDateViewModel>();

            foreach (var g in gro)
            {
                DiaryLogGroupByDateViewModel diaryLogGroup = new DiaryLogGroupByDateViewModel();
                List<DiaryLog> diaLogs = new List<DiaryLog>();

                foreach (var item in g)
                {
                    diaLogs.Add(item);
                }

                diaryLogGroup.DiaryLogDate = g.Key;
                diaryLogGroup.DiaryLogs = diaLogs;

                diaryLogGroups.Add(diaryLogGroup);
            }
            

            diaryLogSearch.DiaryLogGroupByDates = diaryLogGroups.OrderByDescending(p => p.DiaryLogDate).ToPagedList(diaryLogSearch.Page > 0 ? diaryLogSearch.Page - 1 : 0, PageSize);

            return View(diaryLogSearch);
        }
        #endregion

        #region 明細
        public ActionResult Details(int? id)
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DiaryLog diaryLog = _genericRepository.GetById(id.Value);

            if (diaryLog == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<DiaryLog, DiaryLogViewModel>();
            DiaryLogViewModel diaryLogVM = Mapper.Map<DiaryLogViewModel>(diaryLog);

            return View(diaryLogVM);
        }
        #endregion

        #region 新增
        public ActionResult Create()
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            DiaryLogViewModel diaryLogVM = new DiaryLogViewModel();
            diaryLogVM.DiaryLogDate = System.DateTime.Now;

            return View(diaryLogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiaryLogViewModel diaryLogVM)
        {
            DateTime? nowDate = System.DateTime.Now;

            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }


            if (ModelState.IsValid)
            {
                diaryLogVM.UserId = Convert.ToInt32(Session["Id"]);
                diaryLogVM.CreateId = Session["Account"].ToString();
                diaryLogVM.CreateDate = nowDate;
                diaryLogVM.UpdateId = Session["Account"].ToString();
                diaryLogVM.UpdateDate = nowDate;

                Mapper.CreateMap<DiaryLogViewModel,DiaryLog>();
                DiaryLog diaryLog = Mapper.Map<DiaryLog>(diaryLogVM);

                _genericRepository.Insert(diaryLog);
                
                return RedirectToAction("Index");
            }

            return View(diaryLogVM);
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

            DiaryLog diaryLog = _genericRepository.GetById(id.Value);

            if (diaryLog == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<DiaryLog,DiaryLogViewModel > ();
            DiaryLogViewModel diaryLogVM = Mapper.Map<DiaryLogViewModel>(diaryLog);

            return View(diaryLogVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DiaryLogViewModel diaryLogVM)
        {
            if (!IsSessionExist())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {

                diaryLogVM.UpdateId = Session["Account"].ToString();
                diaryLogVM.UpdateDate = System.DateTime.Now;
                Mapper.CreateMap<DiaryLogViewModel, DiaryLog>();
                DiaryLog diaryLog = Mapper.Map<DiaryLog>(diaryLogVM);

                _genericRepository.Edit(diaryLog);

                return RedirectToAction("Index");
            }

            return View(diaryLogVM);
        }
        #endregion

        #region 刪除
        // GET: DiaryLogs/Delete/5
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
            DiaryLog diaryLog = _genericRepository.GetById(id.Value);

            if (diaryLog == null)
            {
                return HttpNotFound();
            }

            Mapper.CreateMap<DiaryLog, DiaryLogViewModel>();
            DiaryLogViewModel diaryLogVM = Mapper.Map<DiaryLogViewModel>(diaryLog);

            return View(diaryLogVM);
        }

        // POST: DiaryLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiaryLog diaryLog = _genericRepository.GetById(id);
            _genericRepository.Delete(diaryLog);

            return RedirectToAction("Index");
        }
        #endregion

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

    }
}
