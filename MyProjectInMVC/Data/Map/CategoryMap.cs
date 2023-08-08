/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Data.Map
{
	public class CategoryMap : IEntityTypeConfiguration<CategoryModel>
	{
		public void Configure(EntityTypeBuilder<CategoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.HasOne(x => x.User);
		}
	}
}
*/