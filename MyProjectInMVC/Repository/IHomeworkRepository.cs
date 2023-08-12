using MyProjectInMVC.Models;

namespace MyProjectInMVC.Repository
{
    public interface IHomeworkRepository
    {
        HomeworkModel Add(HomeworkModel homework);
        List<HomeworkModel> HomeworkList();
        HomeworkModel FindPerId(Guid id);
        bool Delete(Guid id);
        HomeworkModel Edit(HomeworkModel homework);
    }
}
