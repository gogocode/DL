using DL.Models.Service.ServiceModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models.Repository.Class
{
    public class UserAndDiaryLogRepository : IDisposable
    {

        DiaryLogDBEntities _dbContext = null;

        public UserAndDiaryLogRepository(DiaryLogDBEntities context)
        {
            _dbContext = context;
        }

        public UserAndDiaryLogRepository():this(new DiaryLogDBEntities())
        {
        }

        public IQueryable<DiaryLogAndUserSM> GetAllUserJoinDiaryLog()
        {
            IQueryable<DiaryLogAndUserSM> querys = from u in _dbContext.User
                        join d in _dbContext.DiaryLog on u.UserId equals d.UserId
                        select new DiaryLogAndUserSM{
                            UserAccount = u.UserAccount,
                            UserName = u.UserName,
                            UserEmail = u.UserEmail,
                            diaryLog = d
                        };

            return querys;
        }


        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._dbContext != null)
                {
                    this._dbContext.Dispose();
                    this._dbContext = null;
                }
            }
        }

    }
}
