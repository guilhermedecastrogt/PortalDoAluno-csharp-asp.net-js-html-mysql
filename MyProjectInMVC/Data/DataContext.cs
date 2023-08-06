using Microsoft.EntityFrameworkCore;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<UserModel> Users { get; set; }
    }
}
