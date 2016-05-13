using DL.Models;
using DL.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DL.Web.Services
{
    public class AccountService
    {
        GenericRepository<User> _genericRepository = null;

        public AccountService()
        {
           this._genericRepository = new GenericRepository<User>(new DiaryLogDBEntities());
        }

        public bool IsAccountExist(string account)
        {
            User user = _genericRepository.GetAll().Where(x => x.UserAccount == account).FirstOrDefault();

            return user != null;
        }

    }
}