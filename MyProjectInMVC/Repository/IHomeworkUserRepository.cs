using MyProjectInMVC.Models;

namespace MyProjectInMVC.Repository;

public interface IHomeworkUserRepository
{
    HomeworkUserModel Create (HomeworkUserModel model);
    HomeworkUserModel Update (HomeworkUserModel model);
    bool Delete(HomeworkUserModel model);
    List<HomeworkModel> CheckDeleteTrue( List<HomeworkModel> models, Guid userId);
    List<HomeworkModel> CheckDeleteFalse( List<HomeworkModel> models, Guid userId);
}