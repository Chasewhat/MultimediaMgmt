using DevExpress.Mvvm.DataAnnotations;
using System;
using WebMatrix.WebData;
using System.Web.Security;

namespace MultimediaMgmt.ViewModel
{
    [POCOViewModel]
    public class LoginViewModel : BaseViewModel
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public LoginViewModel()
        {
            UserName = Password = "admin";
            //初始化用户信息
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "Id", "LoginName", false);
        }

        public bool Login(ref string result)
        {
            try
            {
                if (string.IsNullOrEmpty(UserName) ||
                    string.IsNullOrEmpty(Password))
                {
                    result = "用户名或密码不能为空";
                    return false;
                }
                if (LoginCheck(UserName, Password))
                {
                    int id = WebSecurity.GetUserId(UserName);
                    Model.Constants.CurrUser = multimediaEntities.UserProfile.Find(id);
                    return true;
                }
                else
                {
                    result = "用户名或密码错误";
                    return false;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }
        public bool LoginCheck(string user, string password)
        {
            ExtendedMembershipProvider provider = Membership.Provider as ExtendedMembershipProvider;
            if (provider == null)
            {
                return false;
            }
            return Membership.ValidateUser(user, password);
        }
    }
}
