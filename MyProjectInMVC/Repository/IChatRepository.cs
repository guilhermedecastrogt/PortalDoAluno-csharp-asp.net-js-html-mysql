using MyProjectInMVC.Controllers;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Repository;

public interface IChatRepository
{
    string Time(DateTime datetime);
    modelIndex Model();
}