using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Data.Map
{
    public class HomeworkMap : IEntityTypeConfiguration<HomeworkModel>
    {
        public void Configure(EntityTypeBuilder<HomeworkModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Category);
        }
    }
}
