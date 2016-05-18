using DL.Models.Repository.Class.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models.Service.DiaryLogs
{
    public class DiaryLogService : IDisposable
    {

        public List<DiaryLog> GetDiaryLogsByUserId(int userId)
        {
            using (DiaryLogRepository _repo = new DiaryLogRepository())
            {
                return _repo.GetAll().Where(x => x.UserId == userId).ToList();
            }
        }

        public List<DiaryLog> GetDiaryLogsByDate(string strDate, int userId)
        {
            DateTime date = Convert.ToDateTime(strDate);

            return GetDiaryLogsByUserId(userId).Where(x => x.DiaryLogDate == date).ToList();
        }

        public List<DateTime> GetDiarysGroupByUserId( int userId,DateTime? dateStart = null,DateTime? dateEnd = null)
        {

            List<DiaryLog> diaryLogs = GetDiaryLogsByUserId(userId);

            if(dateStart != null)
            {
                diaryLogs = diaryLogs.Where(x => x.DiaryLogDate >= dateStart).ToList();
            }

            if (dateEnd != null)
            {
                diaryLogs = diaryLogs.Where(x => x.DiaryLogDate <= dateEnd).ToList();
            }

     
            var gro = from d in diaryLogs
                      group d by d.DiaryLogDate;

            List<DateTime> diaryLogDates = new List<DateTime>();

            foreach (var item in gro)
            {
                DateTime diaryLogDate = item.Key;
                diaryLogDates.Add(diaryLogDate);
            }

            return diaryLogDates;
        }

        public void InsertDiaryLog(DiaryLog diaryLogs, DateTime diaryLogDate, string account, int userId)
        {
            using (DiaryLogRepository _repo = new DiaryLogRepository())
            {

                DiaryLog newDiaryLog = new DiaryLog();

                newDiaryLog.DiaryLogDate = diaryLogDate;
                newDiaryLog.UserId = userId;
                newDiaryLog.DiaryLogItem = diaryLogs.DiaryLogItem;
                newDiaryLog.DiaryLogContents = diaryLogs.DiaryLogContents;
                newDiaryLog.DiaryLogStatus = diaryLogs.DiaryLogStatus;
                newDiaryLog.DiaryLogHours = diaryLogs.DiaryLogHours;
                newDiaryLog.DiaryLogSituation = diaryLogs.DiaryLogSituation;
                newDiaryLog.DiaryLogSolve = diaryLogs.DiaryLogSolve;
                newDiaryLog.CreateDate = DateTime.Now;
                newDiaryLog.CreateId = account;
                newDiaryLog.UpdateDate = DateTime.Now;
                newDiaryLog.UpdateId = account;

                _repo.Insert(newDiaryLog);
            }
        }

        public void UpdateDiaryLog(DiaryLog diaryLog, DiaryLog newDiaryLog, DateTime diaryLogDate, string account, int userId)
        {
            using (DiaryLogRepository _repo = new DiaryLogRepository())
            {

                diaryLog.DiaryLogItem = newDiaryLog.DiaryLogItem;
                diaryLog.DiaryLogContents = newDiaryLog.DiaryLogContents;
                diaryLog.DiaryLogStatus = newDiaryLog.DiaryLogStatus;
                diaryLog.DiaryLogHours = newDiaryLog.DiaryLogHours;
                diaryLog.DiaryLogSituation = newDiaryLog.DiaryLogSituation;
                diaryLog.DiaryLogSolve = newDiaryLog.DiaryLogSolve;
                diaryLog.UpdateDate = DateTime.Now;
                diaryLog.UpdateId = account;

                _repo.Edit(diaryLog);
            }
        }

        public void ModidDiaryLogy(List<DiaryLog> diaryLogs, DateTime diaryLogDate, string account, int userId)
        {

            using (DiaryLogRepository _repo = new DiaryLogRepository())
            {

                foreach (var item in diaryLogs)
                {
                    DiaryLog diaryLog = _repo.GetById(item.DiaryLogId);

                    if (diaryLog != null)
                    {
                        UpdateDiaryLog(diaryLog, item, diaryLogDate, account, userId);
                    }
                    else
                    {
                        InsertDiaryLog(item, diaryLogDate, account, userId);
                    }
                }
            }

        }

        public bool DeleteDiaryLog(int diaryLogId)
        {
            bool result = false;

            using (DiaryLogRepository _repo = new DiaryLogRepository())
            {
                var detail = _repo.GetById(diaryLogId);
                if (detail != null)
                {
                    _repo.Delete(detail);
                    result = true;
                }
            }

            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
