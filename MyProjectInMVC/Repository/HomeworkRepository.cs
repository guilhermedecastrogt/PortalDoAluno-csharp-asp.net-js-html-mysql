using MyProjectInMVC.Data;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.MessageModels;

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
            
            //Delete file
            if (System.IO.File.Exists(homework.FilePath))
            {
                System.IO.File.Delete(homework.FilePath);
            }

            List<MessageHomeworkModel> messages = _context.MessageHomework.Where(x => x.HomeworkId == id).ToList();
            _context.MessageHomework.RemoveRange(messages);
            
            _context.Homeworks.Remove(homework);
            _context.SaveChanges();
            return true;
        }

        public HomeworkModel Edit(HomeworkModel homework)
        {
            HomeworkModel oldHomework = FindPerId(homework.Id);
            if (homework != null && oldHomework != null)
            {
                if (homework.FilePath != null)
                {
                    oldHomework.FilePath = homework.FilePath;
                }
                oldHomework.Title = homework.Title;
                oldHomework.Instructions = homework.Instructions;
                oldHomework.UpdatedAt = DateTime.Now;

                _context.Homeworks.Update(oldHomework);
                _context.SaveChanges();
                return oldHomework;
            }
            
            throw new System.Exception("Erro interno");
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
