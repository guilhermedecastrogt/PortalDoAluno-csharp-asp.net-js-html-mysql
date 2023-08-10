/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyProjectInMVC.Data;
using MyProjectInMVC.Models;
using Newtonsoft.Json;

namespace MyProjectInMVC.Filters
{
    public class CategoryUserPage : ActionFilterAttribute
    {

        private readonly DataContext _dataContext;
        public CategoryUserPage(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string userSession = context.HttpContext.Session.GetString("LoggedUserSession");

            if (string.IsNullOrEmpty(userSession))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {
                       "controller", "Login"
                    },
                    {
                        "action", "Index"
                    }
                });
            }
            else
            {
                UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);
                if (user == null)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {
                       "controller", "Login"
                    },
                    {
                        "action", "Index"
                    }
                });
                }

                UserCategoryModel confirm = _dataContext.UserCategory.FirstOrDefault
                (
                    x => x.UserId == user.Id
             
                );

                if (user.Role != Enums.AdmEnum.Admin)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {
                       "controller", "Home"
                    },
                    {
                        "action", "Index"
                    }
                });
                }
            }
            base.OnActionExecuting(context);
        }
    }
}*/
