using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models.Service.ServiceModels
{
    public class DiaryLogAndUserSM
    {
        public string UserAccount { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public DiaryLog diaryLog { get; set; }
    }
}
