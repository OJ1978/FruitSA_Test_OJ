using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Persistence.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<Products>> GetProductsAsync();
        Task<Products> GetProductByIdAsync(int id);
        Task AddProductAsync(Products product);
        Task UpdateProductAsync(Products product);
        Task DeleteProductAsync(int id);
        Task<byte[]> GetProductImageByIdAsync(int id);
        Task<List<Categories>> GetCategoriesAsync();
        Task<string> GenerateProductCodeAsync();
        Task<string> GetCategoryNameByIdAsync(int categoryId);
    }
}
