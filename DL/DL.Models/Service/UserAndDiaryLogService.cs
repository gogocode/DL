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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
