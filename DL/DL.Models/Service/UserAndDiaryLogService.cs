using DL.Models.Repository.Class;
using DL.Models.Service.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models.Service
{
    public class UserAndDiaryLogService : IDisposable
    {
        public List<DiaryLogAndUserSM> FindAllUserJoinDiaryLog()
        {
            using (UserAndDiaryLogRepository _repo = new UserAndDiaryLogRepository())
            {
                return _repo.GetAllUserJoinDiaryLog().ToList();
            }
        }

        public List<DiaryLogAndUserSM> FindUserJoinDiaryLogByAccountId(string userAccount,string userName)
        {
            List<DiaryLogAndUserSM> diaryLogAndUsers = FindAllUserJoinDiaryLog();

            if (!string.IsNullOrWhiteSpace(userAccount))
            {
                diaryLogAndUsers = diaryLogAndUsers.Where(x => x.UserAccount.Contains(userAccount)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                diaryLogAndUsers = diaryLogAndUsers.Where(x=>x.UserName.Contains(userName)).ToList();
            }

            return diaryLogAndUsers;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
