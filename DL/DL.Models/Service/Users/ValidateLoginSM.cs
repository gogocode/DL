using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.Models.Service.Users
{
    public class ValidateLoginSM
    {
        public int LoginStatus { get; set; }

        public int UserId { get; set; }

        public string UserAccount { get; set; }
    }
}
