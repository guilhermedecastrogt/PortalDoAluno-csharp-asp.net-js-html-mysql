using Microsoft.EntityFrameworkCore;
using MyProjectInMVC.Controllers;
using MyProjectInMVC.Data.Map;
using MyProjectInMVC.Models;
using MyProjectInMVC.Models.ChatModels;
using MyProjectInMVC.Models.MessageModels;

namespace MyProjectInMVC.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserCategoryModel> UserCategory { get; set; }
        public DbSet<HomeworkModel> Homeworks { get; set; }
        public DbSet<HomeworkUserModel> HomeworkUserModel { get; set; }
        public DbSet<MessageHomeworkModel> MessageHomework { get; set; }
        public DbSet<MessageChatModel> Chat { get; set; }
        public DbSet<CategoryModel> Category { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            modelBuilder.ApplyConfiguration(new HomeworkMap());
            modelBuilder.Entity<CategoryModel>()
                .HasIndex(c => c.Slug)
                .IsUnique();
            
            base.OnModelCreating(modelBuilder);
		}
    }
}
