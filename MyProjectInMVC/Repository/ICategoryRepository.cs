using MyProjectInMVC.Models;

namespace MyProjectInMVC.Repository
{
    public interface ICategoryRepository
    {
        CategoryModel Add(CategoryModel category);
        List<CategoryModel> CategoryList();
        CategoryModel FindPerId(Guid? id);
        bool Delete(Guid id);
        CategoryModel Edit(CategoryModel category);
        CategoryModel FindPerSlug(string slug);
    }
}
