using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Services
{
    //Service to Handle all Calls from "Category" Pages
    public class CategoriesService : ICategoriesService
    {
        private readonly Contexts.ApplicationDbContext _context;

        public CategoriesService(Contexts.ApplicationDbContext context)
        {
            _context = context; //Get DB
        } 

        public async Task<IEnumerable<Categories>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync(); //Get All Categories
        }

        public async Task<Categories?> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId); //Get Specific Category
        }

        public async Task AddCategoryAsync(Categories category)
        {
            _context.Categories.Add(category); //Add Category to DB
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Categories category)
        {
            try
            {
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync(); //Update Category
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryExists(category.CategoryId)) //Check if Category doesn't Exist
                {
                    throw new KeyNotFoundException($"Category with ID {category.CategoryId} not found.");
                }
                else
                {
                    //Make sure only one Person editing Records at a Time
                    throw new DbUpdateConcurrencyException("The record you attempted to edit was modified by another user after you got the original value.");
                }
            }
        }

        public async Task<List<Products>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync(); //Get All Products by Category
        }

        public async Task UpdateProductsAsync(List<Products> products)
        {
            _context.Products.UpdateRange(products); //Update Products
            await _context.SaveChangesAsync();
        }

        private async Task<bool> CategoryExists(int categoryId) 
        {
            return await _context.Categories.AnyAsync(e => e.CategoryId == categoryId); //Check if Category exists
        }

        public async Task DeleteCategoryAsync(int id) 
        {
            var category = await _context.Categories.FindAsync(id); //Find Category
            if (category != null)
            {
                _context.Categories.Remove(category); //Delete Category
                await _context.SaveChangesAsync();
            }
        }
    }

}
