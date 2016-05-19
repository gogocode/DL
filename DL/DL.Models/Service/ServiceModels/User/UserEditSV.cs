using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models.Service.ServiceModels.User
{
    public class UserEditSV
    {

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserPassword { get; set; }

        public string UserEmail { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateId { get; set; }

    }
}
