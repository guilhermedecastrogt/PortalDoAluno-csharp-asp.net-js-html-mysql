using Microsoft.EntityFrameworkCore;
using MyProjectInMVC.Data;
using MyProjectInMVC.Enums;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using NuGet.Protocol.Plugins;

namespace MyProjectInMVC.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        public UserRepository(DataContext bancocontext)
        {
            _dataContext = bancocontext;
        }
        public UserModel Add(UserModel user)
        {
            //Export to database
            user.SetPasswordHash();
            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            return user;
        }

        public List<UserModel> ContactsList()
        {
            return _dataContext.Users.ToList();
        }

        public bool Delete(Guid id)
        {
            UserModel userDb = ListPerId(id);

            if (userDb == null)
            {
                throw new System.Exception("Internal Error");
            }

            _dataContext.UserCategory.RemoveRange(_dataContext.UserCategory.Where(x => x.UserId == id));
            _dataContext.Users.Remove(userDb);
            _dataContext.SaveChanges();

            return true;
        }

        public UserModel Edit(UserModel user)
        {
            UserModel userDb = ListPerId(user.Id);

            if (userDb == null)
            {
                throw new System.Exception("Houve um erro interno");
            }

            userDb.Name = user.Name;
            userDb.Email = user.Email;
            userDb.UpdatedAt = DateTime.Now;
            userDb.Role = user.Role;
            userDb.phoneNumber = user.phoneNumber;
            userDb.Cpf = user.Cpf;
            
            _dataContext.Users.Update(userDb);
            _dataContext.SaveChanges();

            return userDb;
        }

        public UserModel ListPerId(Guid id)
        {
            return _dataContext.Users.FirstOrDefault(x => x.Id == id);
        }
        public UserModel FindPerEmail(string email)
        {
            return _dataContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public UserModel ResetPassword(ResetCurrentPasswordModel resetCurrentPassword)
        {
            UserModel userDb = _dataContext.Users.FirstOrDefault(
                x => x.Id == resetCurrentPassword.Id
                &&
                x.Password == resetCurrentPassword.CurrentPassword.HashGenerate()
            );

            if (userDb == null)
            {
                throw new System.Exception("A senha atual está incorreta");
            }
            else
            {
                if(resetCurrentPassword.NewPassword == resetCurrentPassword.CurrentPassword)
                {
                    throw new Exception("Nova senha deve ser diferente da atiga");
                }
                else
                {
                    userDb.SetNewPassword(resetCurrentPassword.NewPassword);
                    userDb.UpdatedAt = DateTime.Now;

                    _dataContext.Users.Update(userDb);
                    _dataContext.SaveChanges();

                    return userDb;
                }
            }
        }

        public bool UserCategoryAdd(List<Guid> selectedCategoryIds, Guid user, CategoryLevelEnum level)
        {
            try
            {
                _dataContext.UserCategory.RemoveRange(_dataContext.UserCategory.Where(x => x.UserId == user));

                foreach (var categoryId in selectedCategoryIds)
                {
                    _dataContext.UserCategory.Add(new UserCategoryModel { UserId = user, CategoryId = categoryId, Level = level});
                }

                _dataContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }            
        }
    }
}