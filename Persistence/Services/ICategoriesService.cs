using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Services
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Categories>> GetCategoriesAsync();
        Task<Categories?> GetCategoryByIdAsync(int categoryId);
        Task AddCategoryAsync(Categories category);
        Task UpdateCategoryAsync(Categories category);
        Task<List<Products>> GetProductsByCategoryIdAsync(int categoryId);
        Task UpdateProductsAsync(List<Products> products);
        Task DeleteCategoryAsync(int id);

    }
}
