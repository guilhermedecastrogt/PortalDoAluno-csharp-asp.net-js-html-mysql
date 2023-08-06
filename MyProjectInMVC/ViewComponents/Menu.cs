using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyProjectInMVC.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MyProjectInMVC.ViewComponents
{
    public class Menu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userSession = HttpContext.Session.GetString("LoggedUserSession");
            
            if (string.IsNullOrEmpty(userSession))
            {
                return null;
            }

            UserModel user = JsonConvert.DeserializeObject<UserModel>(userSession);

            return View(user);
        }
    }
}
