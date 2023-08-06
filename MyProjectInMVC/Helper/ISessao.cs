using MyProjectInMVC.Models;

namespace MyProjectInMVC.Helper
{
    public interface ISessao
    {
        void CreateUserSession(UserModel user);
        void RemoveUserSession();
        UserModel FindSession();
        public bool IfLogged();
    }
}
