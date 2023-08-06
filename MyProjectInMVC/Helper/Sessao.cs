using MyProjectInMVC.Models;
using Newtonsoft.Json;


namespace MyProjectInMVC.Helper
{
    public class Sessao : ISessao
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public Sessao(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void CreateUserSession(UserModel user)
        {
            string value = JsonConvert.SerializeObject(user);
            _contextAccessor.HttpContext.Session.SetString("LoggedUserSession", value);
        }

        public UserModel FindSession()
        {
            string userSession = _contextAccessor.HttpContext.Session.GetString("LoggedUserSession");

            if(string.IsNullOrEmpty(userSession))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<UserModel>(userSession);
            }
        }

        public bool IfLogged()
        {
            string userSession = _contextAccessor.HttpContext.Session.GetString("LoggedUserSession");

            if (string.IsNullOrEmpty(userSession))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void RemoveUserSession()
        {
            _contextAccessor.HttpContext.Session.Remove("LoggedUserSession");
        }
    }
}
