using MyProjectInMVC.Data;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Repository
{
    public class HomeworkRepository : IHomeworkRepository
    {
        private readonly DataContext _context;
        public HomeworkRepository(DataContext context)
        {
            _context = context;
        }
        public HomeworkModel Add(HomeworkModel homework)
        {
            _context.Homeworks.Add(homework);
            _context.SaveChanges();
            return homework;
        }

        public bool Delete(Guid id)
        {
            HomeworkModel homework = FindPerId(id);
            if(homework == null)
            {
                return false;
            }
            _context.Homeworks.Remove(homework);
            _context.SaveChanges();
            return true;
        }

        public HomeworkModel Edit(HomeworkModel homework)
        {
            throw new NotImplementedException();
        }

        public HomeworkModel FindPerId(Guid id)
        {
            HomeworkModel homework = _context.Homeworks.FirstOrDefault(x => x.Id == id);
            return homework;
        }

        public List<HomeworkModel> HomeworkList()
        {
            return _context.Homeworks.ToList();
        }
    }
}
