using DL.Models.Repository.Class.Base;
using DL.Models.Service.ServiceModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DL.Models.Service.Users
{
    public class UserService : IDisposable
    {
        public bool IsUserAccountExist(string userAccount)
        {
            using (UserRepository _repo = new UserRepository())
            {
                var users = _repo.GetAll().Where(x => x.UserAccount.Equals(userAccount)).ToList();

                return users.Count() > 0;
            }
        }

        public string GetUserNameById(int userId)
        {
            return GetUserById(userId).UserName;
        }

        public string GetUserAccountById(int userId)
        {
            return GetUserById(userId).UserAccount;
        }

        public User GetUserById(int userId)
        {
            using (UserRepository _repo = new UserRepository())
            {
                return _repo.GetById(userId);
            }
        }

        public List<User> FindUsersByAccount(string account)
        {
            IQueryable<User> users = GetAllUsers();

            if (!string.IsNullOrWhiteSpace(account))
            {
                users = GetAllUsers().Where(x => x.UserAccount.Contains(account));
            }

            return users.ToList();
        }

        public List<User> FindAllUsers()
        {
            return GetAllUsers().ToList();
        }
        

        public IQueryable<User> GetAllUsers()
        {
            UserRepository _repo = new UserRepository();
      
            return _repo.GetAll();
        }

        public List<User> FindUsers(string account,string name)
        {
            IQueryable<User> users = GetAllUsers();

            if (!string.IsNullOrWhiteSpace(account))
            {
                users = users.Where(x => x.UserAccount.Contains(account));
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                users = users.Where(x => x.UserName.Contains(name));
            }

            return users.ToList();
        }


        public User GetUser(string userAccount, string userPassword)
        {
            using (UserRepository _repo = new UserRepository())
            {
                List<User> users = _repo.GetAll().Where(x => x.UserAccount.Equals(userAccount) && x.UserPassword.Equals(userPassword)).ToList();

                return users.FirstOrDefault();
            }
        }

        /// <summary>
        /// 編輯User
        /// </summary>
        /// <param name="userEditSV"></param>
        public void ModifyUser(UserEditSV userEditSV)
        {
            using (UserRepository _repo = new UserRepository())
            {
                User user = GetUserById(userEditSV.UserId);

                user.UserName = userEditSV.UserName;
                user.UserPassword = userEditSV.UserPassword;
                user.UserEmail = userEditSV.UserEmail;
                user.UpdateDate = userEditSV.UpdateDate;
                user.UpdateId = userEditSV.UpdateId;

                _repo.Edit(user);
            }
        }

        /// <summary>
        /// 驗證使用者是否登入成功
        /// </summary>
        /// <param name="userAccount">登入帳號</param>
        /// <param name="userPassword">登入密碼</param>
        /// <returns>loginStatus為0 帳號或密碼輸入錯誤 loginStatus為1 帳號未啟動 loginStatus為2 登入正常</returns>
        public ValidateLoginSM ValidateLogin(string userAccount, string userPassword)
        {

            User user = GetUser(userAccount, userPassword);
            ValidateLoginSM validateLoginSV = new ValidateLoginSM();

            if (user != null)
            {
                if (user.UserStatus == "0" || string.IsNullOrEmpty(user.UserStatus))
                {
                    validateLoginSV.LoginStatus = 1;
                    return validateLoginSV;
                }

                validateLoginSV.LoginStatus = 2;
                validateLoginSV.UserId = user.UserId;
                validateLoginSV.UserAccount = user.UserAccount;
                return validateLoginSV;
            }

            validateLoginSV.LoginStatus = 0;
            return validateLoginSV;
        }

        public void StartUp(int id)
        {
            using (UserRepository _repo = new UserRepository())
            {
                User user = _repo.GetById(id);
                user.UserStatus = "1";
                _repo.Edit(user);
            }
        }

        public void EndUp(int id)
        {
            using (UserRepository _repo = new UserRepository())
            {
                User user = _repo.GetById(id);
                user.UserStatus = "0";
                _repo.Edit(user);
            }
        }

        public void DeleteUserById(int id)
        {
            using (UserRepository _repo = new UserRepository())
            {
                User user = _repo.GetById(id);
                _repo.Delete(user);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
