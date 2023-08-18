using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectInMVC.Data;
using MyProjectInMVC.Models;

namespace MyProjectInMVC.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public CategoryModel Add(CategoryModel category)
        {
            _context.Category.Add(category);
            _context.SaveChanges();
            return category;
        }
        public List<CategoryModel> CategoryList()
        {
            return _context.Category.ToList();
        }
        public CategoryModel FindPerId(Guid? id)
        {
            if (id != null)
            {
                CategoryModel category = _context.Category.FirstOrDefault(x => x.Id == id);
                return category;
            }
            throw new System.Exception("Houve um erro interno");
        }
        public bool Delete(Guid id)
        {
            CategoryModel category = FindPerId(id);

            if (category == null)
            {
                return false;
            }

            _context.Category.Remove(category);
            _context.UserCategory.RemoveRange(_context.UserCategory.Where(x => x.CategoryId == id));
            _context.SaveChanges();
            return true;
        }
        public CategoryModel Edit(CategoryModel category)
        {
            var oldCategory = FindPerId(category.Id);

            if (oldCategory == null)
            {
                throw new System.Exception("Houve um erro interno");
            }

            oldCategory.Name = category.Name;
            oldCategory.Slug = category.Slug;

            _context.Category.Update(oldCategory);
            _context.SaveChanges();
            return oldCategory;

        }
    }
}
