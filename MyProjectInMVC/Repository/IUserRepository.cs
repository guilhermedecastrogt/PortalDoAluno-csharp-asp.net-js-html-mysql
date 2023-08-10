using MyProjectInMVC.Enums;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Repository
{
    public interface IUserRepository
    {
        UserModel FindPerEmail(string email);
        UserModel ListPerId(Guid id);
        List<UserModel> ContactsList();
        UserModel Add(UserModel user);
        UserModel Edit(UserModel user);
        bool Delete(Guid id);
        UserModel ResetPassword (ResetCurrentPasswordModel resetCurrentPassword);
        bool UserCategoryAdd (List<Guid> selectedCategoryIds, Guid user, CategoryLevelEnum level);
    }
}