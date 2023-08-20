using MyProjectInMVC.Data;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Repository;

public class HomeworkUserRepository : IHomeworkUserRepository
{
    private readonly DataContext _context;
    public HomeworkUserRepository(DataContext context)
    {
        _context = context;
    }
    
    public HomeworkUserModel Create(HomeworkUserModel model)
    {
        _context.HomeworkUserModel.Add(model);
        _context.SaveChanges();
        return model;
    }

    public HomeworkUserModel Update(HomeworkUserModel model)
    {
        HomeworkUserModel find = _context.HomeworkUserModel.FirstOrDefault(x => x.UserId == model.UserId && x.HomeworkId == model.HomeworkId);
        _context.HomeworkUserModel.Update(find);
        _context.SaveChanges();
        return find;
    }

    public bool Delete(HomeworkUserModel model)
    {
        HomeworkUserModel find =
            _context.HomeworkUserModel.FirstOrDefault(x =>
                x.UserId == model.UserId && x.HomeworkId == model.HomeworkId);
        if (find != null)
        {
            _context.HomeworkUserModel.Remove(find);
            return true;
        }

        return false;
    }

    public List<HomeworkModel> CheckDeleteTrue(List<HomeworkModel> models, Guid userId)
    {
        List<HomeworkModel> updatedModels = new List<HomeworkModel>();

        foreach (HomeworkModel item in models)
        {
            HomeworkUserModel? HomeworkTrue = _context.HomeworkUserModel.FirstOrDefault(x =>
                x.UserId == userId &&
                x.HomeworkId == item.Id &&
                x.Status == true
            );

            if (HomeworkTrue == null)
            {
                updatedModels.Add(item);
            }
        }

        return updatedModels;
    }
    
    public List<HomeworkModel> CheckDeleteFalse(List<HomeworkModel> models, Guid userId)
    {
        List<HomeworkModel> updatedModels = new List<HomeworkModel>();

        foreach (HomeworkModel item in models)
        {
            HomeworkUserModel? HomeworkTrue = _context.HomeworkUserModel.FirstOrDefault(x =>
                x.UserId == userId &&
                x.HomeworkId == item.Id &&
                x.Status == true
            );

            if (HomeworkTrue != null)
            {
                updatedModels.Add(item);
            }
        }

        return updatedModels;
    }
}