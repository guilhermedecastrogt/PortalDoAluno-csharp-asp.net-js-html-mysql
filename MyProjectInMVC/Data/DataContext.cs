using Microsoft.EntityFrameworkCore;
//using MyProjectInMVC.Data.Map;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserCategoryModel> UserCategory { get; set; }
        public DbSet<HomeworkModel> Homework { get; set; }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.ApplyConfiguration(new CategoryMap());

            base.OnModelCreating(modelBuilder);
		}*/
    }
}
