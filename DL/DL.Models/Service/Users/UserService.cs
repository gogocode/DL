using DL.Models.Repository.Class.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DL.Models.Service.Users
{
    public class UserService :IDisposable
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

        public User GetUserById(int userId)
        {
            using (UserRepository _repo = new UserRepository())
            {
                return _repo.GetById(userId);
            } 
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
        /// 驗證使用者是否登入成功
        /// </summary>
        /// <param name="userAccount">登入帳號</param>
        /// <param name="userPassword">登入密碼</param>
        /// <returns>loginStatus為0 帳號或密碼輸入錯誤 loginStatus為1 帳號未啟動 loginStatus為2 登入正常</returns>
        public ValidateLoginSV ValidateLogin(string userAccount, string userPassword)
        {

            //驗證
            //檢查 Username, Password 是否正確
            User user = GetUser( userAccount,  userPassword);
            ValidateLoginSV validateLoginSV = new ValidateLoginSV();

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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
