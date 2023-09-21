using Microsoft.EntityFrameworkCore;
using MyProjectInMVC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MyProjectInMVC.Repository;
using MyProjectInMVC.Helper;

namespace MyProjectInMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                
                //string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
                string? connectionString = Environment.GetEnvironmentVariable("ConnectionStringName");
                connectionString = "server=212.192.29.71;user=seusitep_portaldoalunouser;database=seusitep_PortalDoAluno;port=3306;password=123321Qwa.123321";
                
                builder.Services.AddDbContext<DataContext>
                    (options => options.UseMySQL(connectionString));


                builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                builder.Services.AddScoped<IUserRepository, UserRepository>();
                builder.Services.AddScoped<ISessao, Sessao>();
                builder.Services.AddScoped<IEmail, Email>();
                builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
                builder.Services.AddScoped<IHomeworkRepository, HomeworkRepository>();
                builder.Services.AddScoped<IHomeworkUserRepository, HomeworkUserRepository>();
                builder.Services.AddScoped<IChatRepository, ChatRepository>();

                builder.Services.AddSession(o =>
                {
                    o.Cookie.HttpOnly = true;
                    o.Cookie.IsEssential = true;
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();
        }
    }
}