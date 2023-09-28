using MyProjectInMVC.Data;
using MyProjectInMVC.Helper;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.MessageModels;

namespace MyProjectInMVC.Repository
{
    public class HomeworkRepository : IHomeworkRepository
    {
        private readonly DataContext _context;
        private readonly IFtpUploader _ftpUploader;
        private readonly IConfiguration _configuration;
        public HomeworkRepository(DataContext context, IFtpUploader ftpUploader, IConfiguration configuration)
        {
            _context = context;
            _ftpUploader = ftpUploader;
            _configuration = configuration;
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
            string? path = Path.GetExtension(homework.FilePath);
            if (path != null)
            {
                FtpConnection model = new FtpConnection(_configuration);
                model.ftpServerUrl = model.ftpServerUrl + "/homeworks/";
                model.remoteFileName = homework.Id + path;
                bool check = _ftpUploader.DeleteFile(model);
                if (!check)
                {
                    return false;
                }
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
